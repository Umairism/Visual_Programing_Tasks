using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Auth_WebForms.DataAccess
{
    /// <summary>
    /// Data Access Layer using ADO.NET Connectionless (Disconnected) Architecture
    /// Uses DataAdapter and DataSet for all database operations
    /// </summary>
    public class UserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AuthDbConnection"].ConnectionString;
        }

        #region Connectionless Authentication

        /// <summary>
        /// Authenticate user using DataAdapter and DataSet (Connectionless approach)
        /// </summary>
        public DataTable AuthenticateUser(string username, string passwordHash)
        {
            DataTable userTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    // Create SqlCommand for stored procedure
                    using (SqlCommand command = new SqlCommand("sp_AuthenticateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        // Add return value parameter
                        SqlParameter returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(returnValue);

                        // Use DataAdapter for connectionless approach
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill DataTable without managing connection manually
                            adapter.Fill(userTable);

                            // Check return value
                            int result = Convert.ToInt32(returnValue.Value);
                            
                            if (result == 1 && userTable.Rows.Count > 0)
                            {
                                // Authentication successful
                                return userTable;
                            }
                            else if (result == -1)
                            {
                                throw new Exception("Account is locked or inactive. Please contact administrator.");
                            }
                            else
                            {
                                throw new Exception("Invalid username or password.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Authentication failed: " + ex.Message, ex);
            }
        }

        #endregion

        #region Connectionless User Registration

        /// <summary>
        /// Register new user using DataAdapter (Connectionless approach)
        /// </summary>
        public int RegisterUser(string username, string email, string passwordHash, string fullName, string roleName = "User")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@RoleName", roleName);

                        // Add return value parameter
                        SqlParameter returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(returnValue);

                        // Use DataAdapter for connectionless approach
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);

                            // Get return value
                            int result = Convert.ToInt32(returnValue.Value);

                            if (result == -1)
                            {
                                throw new Exception("Username already exists.");
                            }
                            else if (result == -2)
                            {
                                throw new Exception("Email already exists.");
                            }
                            else if (result == -99)
                            {
                                throw new Exception("An error occurred during registration.");
                            }

                            return result; // Returns new UserId
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Registration failed: " + ex.Message, ex);
            }
        }

        #endregion

        #region Connectionless User Management

        /// <summary>
        /// Get all users using DataAdapter and DataSet
        /// </summary>
        public DataSet GetAllUsers()
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Use DataAdapter to fill DataSet (connectionless)
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataSet, "Users");
                        }
                    }
                }

                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve users: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get user by username using DataAdapter
        /// </summary>
        public DataRow GetUserByUsername(string username)
        {
            DataTable userTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
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
                        WHERE u.Username = @Username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        // Use DataAdapter for connectionless approach
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(userTable);

                            if (userTable.Rows.Count > 0)
                            {
                                return userTable.Rows[0];
                            }

                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve user: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Update user status (Active/Inactive) using DataAdapter
        /// </summary>
        public bool UpdateUserStatus(int userId, bool isActive)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_UpdateUserStatus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        SqlParameter returnValue = new SqlParameter("@ReturnValue", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(returnValue);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);

                            int result = Convert.ToInt32(returnValue.Value);
                            return result > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update user status: " + ex.Message, ex);
            }
        }

        #endregion

        #region Connectionless Role Management

        /// <summary>
        /// Get user roles using DataAdapter
        /// </summary>
        public DataTable GetUserRoles(string username)
        {
            DataTable rolesTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetUserRoles", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);

                        // Use DataAdapter for connectionless approach
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(rolesTable);
                        }
                    }
                }

                return rolesTable;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve user roles: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get all available roles using DataAdapter
        /// </summary>
        public DataTable GetAllRoles()
        {
            DataTable rolesTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT RoleId, RoleName, Description FROM Roles ORDER BY RoleName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use DataAdapter for connectionless approach
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(rolesTable);
                        }
                    }
                }

                return rolesTable;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve roles: " + ex.Message, ex);
            }
        }

        #endregion

        #region DataSet Update Operations

        /// <summary>
        /// Demonstrates DataSet update with DataAdapter (Disconnected architecture)
        /// </summary>
        public bool UpdateUserWithDataSet(int userId, string email, string fullName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string selectQuery = "SELECT UserId, Email, FullName FROM Users WHERE UserId = @UserId";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {
                        // Add parameter for SELECT
                        adapter.SelectCommand.Parameters.AddWithValue("@UserId", userId);

                        // Create command builder to automatically generate UPDATE command
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Fill DataSet
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet, "Users");

                        if (dataSet.Tables["Users"].Rows.Count > 0)
                        {
                            // Modify data in disconnected mode
                            DataRow row = dataSet.Tables["Users"].Rows[0];
                            row["Email"] = email;
                            row["FullName"] = fullName;

                            // Update database (connectionless update)
                            adapter.Update(dataSet, "Users");

                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update user: " + ex.Message, ex);
            }
        }

        #endregion

        #region Bulk Operations with DataTable

        /// <summary>
        /// Demonstrates bulk insert using DataTable and DataAdapter
        /// </summary>
        public void BulkInsertUsers(DataTable usersTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string selectQuery = "SELECT UserId, Username, Email, PasswordHash, FullName, IsActive FROM Users WHERE 1=0";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet, "Users");

                        // Merge new users into DataSet
                        dataSet.Tables["Users"].Merge(usersTable);

                        // Batch update to database
                        adapter.UpdateBatchSize = 100; // Process 100 records at a time
                        adapter.Update(dataSet, "Users");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Bulk insert failed: " + ex.Message, ex);
            }
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
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
