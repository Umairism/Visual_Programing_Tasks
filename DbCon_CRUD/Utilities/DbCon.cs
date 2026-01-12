using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DbCon_CRUD.Utilities
{
    /// <summary>
    /// Centralized Database Connection Class (DbCon)
    /// Handles ALL database operations through static methods
    /// Used directly by presentation layer - no separate BLL or DAL
    /// </summary>
    public static class DbCon
    {
        // Centralized connection string
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["InventoryDBConnection"].ConnectionString;

        #region Connection Management

        /// <summary>
        /// Get a new database connection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Test database connectivity
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Execute Methods

        /// <summary>
        /// Execute non-query command (INSERT, UPDATE, DELETE)
        /// Returns number of rows affected
        /// </summary>
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute scalar query (COUNT, MAX, SUM, etc.)
        /// Returns single value
        /// </summary>
        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Execute reader query (SELECT)
        /// Returns SqlDataReader - caller must dispose
        /// </summary>
        public static SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            conn.Open();
            // CommandBehavior.CloseConnection ensures connection closes when reader closes
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Execute query and return DataTable
        /// Convenient for binding to GridView
        /// </summary>
        public static DataTable ExecuteDataTable(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Execute query and return DataSet (multiple tables)
        /// </summary>
        public static DataSet ExecuteDataSet(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        #endregion

        #region Parameter Helpers

        /// <summary>
        /// Create SQL parameter with value
        /// </summary>
        public static SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Create SQL parameter with specific type
        /// </summary>
        public static SqlParameter CreateParameter(string name, SqlDbType type, object value)
        {
            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = type,
                Value = value ?? DBNull.Value
            };
        }

        /// <summary>
        /// Create OUTPUT parameter
        /// </summary>
        public static SqlParameter CreateOutputParameter(string name, SqlDbType type)
        {
            return new SqlParameter
            {
                ParameterName = name,
                SqlDbType = type,
                Direction = ParameterDirection.Output
            };
        }

        #endregion

        #region Transaction Support

        /// <summary>
        /// Execute multiple commands in a transaction
        /// </summary>
        public static bool ExecuteTransaction(params Action<SqlConnection, SqlTransaction>[] operations)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (var operation in operations)
                    {
                        operation(conn, transaction);
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Check if record exists
        /// </summary>
        public static bool RecordExists(string tableName, string columnName, object value)
        {
            string query = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @Value";
            int count = Convert.ToInt32(ExecuteScalar(query, CreateParameter("@Value", value)));
            return count > 0;
        }

        /// <summary>
        /// Get next identity value
        /// </summary>
        public static int GetNextIdentity(string tableName)
        {
            string query = $"SELECT ISNULL(MAX(CAST(SUBSTRING(COLUMN_NAME, PATINDEX('%[0-9]%', COLUMN_NAME), LEN(COLUMN_NAME)) AS INT)), 0) + 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName AND COLUMN_NAME LIKE '%Id'";
            return Convert.ToInt32(ExecuteScalar(query, CreateParameter("@TableName", tableName)));
        }

        /// <summary>
        /// Get record count
        /// </summary>
        public static int GetRecordCount(string tableName, string whereClause = "")
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";
            if (!string.IsNullOrWhiteSpace(whereClause))
            {
                query += $" WHERE {whereClause}";
            }

            return Convert.ToInt32(ExecuteScalar(query));
        }

        /// <summary>
        /// Check if table exists
        /// </summary>
        public static bool TableExists(string tableName)
        {
            string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName";
            int count = Convert.ToInt32(ExecuteScalar(query, CreateParameter("@TableName", tableName)));
            return count > 0;
        }

        #endregion
    }
}
