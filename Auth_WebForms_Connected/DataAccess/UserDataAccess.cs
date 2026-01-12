using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Auth_WebForms_Connected.DataAccess
{
    /// <summary>
    /// Data Access Layer using ADO.NET Connection-Oriented (Connected) Architecture
    /// Uses SqlConnection, SqlCommand, and SqlDataReader with manual connection management
    /// </summary>
    public class UserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AuthDbConnection"].ConnectionString;
        }

        #region Connection-Oriented Authentication

        /// <summary>
        /// Authenticate user using SqlConnection and SqlDataReader (Connection-oriented approach)
        /// Manually opens and closes the connection
        /// </summary>
        public UserInfo AuthenticateUser(string username, string passwordHash)
        {
            UserInfo userInfo = null;

            // Create connection - must be manually opened and closed
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Manually open the connection
                    connection.Open();

                    // Create command
                    using (SqlCommand command = new SqlCommand(@"
                        SELECT 
                            u.UserId,
                            u.Username,
                            u.Email,
                            u.FullName,
                            u.IsActive,
                            u.IsLockedOut,
                            u.LockoutEndDate,
                            u.FailedLoginAttempts,
                            STUFF((
                                SELECT ',' + r.RoleName
                                FROM UserRoles ur
                                INNER JOIN Roles r ON ur.RoleId = r.RoleId
                                WHERE ur.UserId = u.UserId
                                FOR XML PATH('')
                            ), 1, 1, '') AS Roles
                        FROM Users u
                        WHERE u.Username = @Username AND u.PasswordHash = @PasswordHash
                    ", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        // Use SqlDataReader for connection-oriented reading
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                                bool isLockedOut = reader.GetBoolean(reader.GetOrdinal("IsLockedOut"));
                                DateTime? lockoutEndDate = reader.IsDBNull(reader.GetOrdinal("LockoutEndDate"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("LockoutEndDate"));

                                // Check if account is locked
                                if (isLockedOut && lockoutEndDate.HasValue && lockoutEndDate.Value > DateTime.Now)
                                {
                                    throw new Exception("Account is locked. Please try again after " + 
                                        lockoutEndDate.Value.ToString("MMM dd, yyyy HH:mm"));
                                }

                                if (!isActive)
                                {
                                    throw new Exception("Account is inactive. Please contact administrator.");
                                }

                                // Read user information
                                userInfo = new UserInfo
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    IsActive = isActive,
                                    Roles = reader.IsDBNull(reader.GetOrdinal("Roles")) 
                                        ? "" 
                                        : reader.GetString(reader.GetOrdinal("Roles"))
                                };

                                // Close reader to execute update
                                reader.Close();

                                // Update last login date using the same connection
                                UpdateLastLogin(connection, userInfo.UserId);

                                return userInfo;
                            }
                            else
                            {
                                // Failed login - increment failed attempts
                                reader.Close();
                                IncrementFailedLoginAttempts(connection, username);
                                throw new Exception("Invalid username or password.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Authentication failed: " + ex.Message, ex);
                }
                finally
                {
                    // Ensure connection is closed
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Update last login date using connection-oriented approach
        /// </summary>
        private void UpdateLastLogin(SqlConnection connection, int userId)
        {
            using (SqlCommand command = new SqlCommand(@"
                UPDATE Users 
                SET LastLoginDate = @LastLoginDate,
                    FailedLoginAttempts = 0,
                    IsLockedOut = 0,
                    LockoutEndDate = NULL
                WHERE UserId = @UserId
            ", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@LastLoginDate", DateTime.Now);
                command.ExecuteNonQuery(); // Execute without returning data
            }
        }

        /// <summary>
        /// Increment failed login attempts using connection-oriented approach
        /// </summary>
        private void IncrementFailedLoginAttempts(SqlConnection connection, string username)
        {
            using (SqlCommand command = new SqlCommand(@"
                UPDATE Users 
                SET FailedLoginAttempts = FailedLoginAttempts + 1,
                    IsLockedOut = CASE WHEN FailedLoginAttempts >= 4 THEN 1 ELSE 0 END,
                    LockoutEndDate = CASE WHEN FailedLoginAttempts >= 4 
                                     THEN DATEADD(MINUTE, 30, GETDATE()) 
                                     ELSE NULL END
                WHERE Username = @Username
            ", connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Connection-Oriented User Registration

        /// <summary>
        /// Register new user using SqlConnection and ExecuteNonQuery (Connection-oriented)
        /// </summary>
        public int RegisterUser(string username, string email, string passwordHash, string fullName, string roleName = "User")
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Manually open connection
                    connection.Open();

                    // Start transaction for data consistency
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Check if username exists
                            using (SqlCommand checkCommand = new SqlCommand(
                                "SELECT COUNT(*) FROM Users WHERE Username = @Username", 
                                connection, transaction))
                            {
                                checkCommand.Parameters.AddWithValue("@Username", username);
                                int count = (int)checkCommand.ExecuteScalar(); // Get single value
                                
                                if (count > 0)
                                {
                                    throw new Exception("Username already exists.");
                                }
                            }

                            // Check if email exists
                            using (SqlCommand checkCommand = new SqlCommand(
                                "SELECT COUNT(*) FROM Users WHERE Email = @Email", 
                                connection, transaction))
                            {
                                checkCommand.Parameters.AddWithValue("@Email", email);
                                int count = (int)checkCommand.ExecuteScalar();
                                
                                if (count > 0)
                                {
                                    throw new Exception("Email already exists.");
                                }
                            }

                            // Insert new user
                            int newUserId;
                            using (SqlCommand insertCommand = new SqlCommand(@"
                                INSERT INTO Users (Username, Email, PasswordHash, FullName, IsActive)
                                VALUES (@Username, @Email, @PasswordHash, @FullName, 1);
                                SELECT SCOPE_IDENTITY();
                            ", connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@Username", username);
                                insertCommand.Parameters.AddWithValue("@Email", email);
                                insertCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                                insertCommand.Parameters.AddWithValue("@FullName", fullName);
                                
                                newUserId = Convert.ToInt32(insertCommand.ExecuteScalar());
                            }

                            // Get role ID
                            int roleId;
                            using (SqlCommand roleCommand = new SqlCommand(
                                "SELECT RoleId FROM Roles WHERE RoleName = @RoleName", 
                                connection, transaction))
                            {
                                roleCommand.Parameters.AddWithValue("@RoleName", roleName);
                                object result = roleCommand.ExecuteScalar();
                                
                                if (result == null)
                                {
                                    throw new Exception("Invalid role name.");
                                }
                                
                                roleId = Convert.ToInt32(result);
                            }

                            // Assign role to user
                            using (SqlCommand roleAssignCommand = new SqlCommand(
                                "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)", 
                                connection, transaction))
                            {
                                roleAssignCommand.Parameters.AddWithValue("@UserId", newUserId);
                                roleAssignCommand.Parameters.AddWithValue("@RoleId", roleId);
                                roleAssignCommand.ExecuteNonQuery();
                            }

                            // Commit transaction
                            transaction.Commit();
                            return newUserId;
                        }
                        catch
                        {
                            // Rollback on error
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Registration failed: " + ex.Message, ex);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion

        #region Connection-Oriented User Management

        /// <summary>
        /// Get all users using SqlDataReader (Connection-oriented)
        /// </summary>
        public List<UserInfo> GetAllUsers()
        {
            List<UserInfo> users = new List<UserInfo>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"
                        SELECT 
                            u.UserId,
                            u.Username,
                            u.Email,
                            u.FullName,
                            u.IsActive,
                            u.CreatedDate,
                            u.LastLoginDate,
                            u.FailedLoginAttempts,
                            u.IsLockedOut,
                            u.LockoutEndDate,
                            STUFF((
                                SELECT ',' + r.RoleName
                                FROM UserRoles ur
                                INNER JOIN Roles r ON ur.RoleId = r.RoleId
                                WHERE ur.UserId = u.UserId
                                FOR XML PATH('')
                            ), 1, 1, '') AS Roles
                        FROM Users u
                        ORDER BY u.CreatedDate DESC
                    ", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserInfo user = new UserInfo
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4),
                                    CreatedDate = reader.GetDateTime(5),
                                    LastLoginDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    FailedLoginAttempts = reader.GetInt32(7),
                                    IsLockedOut = reader.GetBoolean(8),
                                    LockoutEndDate = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                    Roles = reader.IsDBNull(10) ? "" : reader.GetString(10)
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return users;
        }

        /// <summary>
        /// Get user by username using SqlDataReader
        /// </summary>
        public UserInfo GetUserByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"
                        SELECT 
                            u.UserId,
                            u.Username,
                            u.Email,
                            u.FullName,
                            u.IsActive,
                            u.CreatedDate,
                            u.LastLoginDate,
                            STUFF((
                                SELECT ',' + r.RoleName
                                FROM UserRoles ur
                                INNER JOIN Roles r ON ur.RoleId = r.RoleId
                                WHERE ur.UserId = u.UserId
                                FOR XML PATH('')
                            ), 1, 1, '') AS Roles
                        FROM Users u
                        WHERE u.Username = @Username
                    ", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserInfo
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4),
                                    CreatedDate = reader.GetDateTime(5),
                                    LastLoginDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                    Roles = reader.IsDBNull(7) ? "" : reader.GetString(7)
                                };
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Update user status using ExecuteNonQuery (Connection-oriented)
        /// </summary>
        public bool UpdateUserStatus(int userId, bool isActive)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"
                        UPDATE Users 
                        SET IsActive = @IsActive,
                            IsLockedOut = 0,
                            LockoutEndDate = NULL,
                            FailedLoginAttempts = 0
                        WHERE UserId = @UserId
                    ", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Get user count using ExecuteScalar (Connection-oriented)
        /// </summary>
        public int GetUserCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users", connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Get active user count using ExecuteScalar
        /// </summary>
        public int GetActiveUserCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(
                        "SELECT COUNT(*) FROM Users WHERE IsActive = 1", connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        #endregion

        #region Connection-Oriented Role Management

        /// <summary>
        /// Get user roles using SqlDataReader
        /// </summary>
        public List<string> GetUserRoles(string username)
        {
            List<string> roles = new List<string>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(@"
                        SELECT r.RoleName
                        FROM Users u
                        INNER JOIN UserRoles ur ON u.UserId = ur.UserId
                        INNER JOIN Roles r ON ur.RoleId = r.RoleId
                        WHERE u.Username = @Username AND u.IsActive = 1
                    ", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                roles.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return roles;
        }

        #endregion

        #region Test Connection

        /// <summary>
        /// Test database connection
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// User Information Model
    /// </summary>
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public string Roles { get; set; }
    }
}
