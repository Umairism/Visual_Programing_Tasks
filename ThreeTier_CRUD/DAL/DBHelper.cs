using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ThreeTier_CRUD.DAL
{
    /// <summary>
    /// Database Helper class for common database operations
    /// Provides connection management and parameter handling
    /// </summary>
    public static class DBHelper
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["ThreeTierDBConnection"].ConnectionString;

        /// <summary>
        /// Get database connection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Execute non-query command (INSERT, UPDATE, DELETE)
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
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
        /// Execute scalar query (COUNT, MAX, etc.)
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
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
        /// </summary>
        public static SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            conn.Open();
            // CommandBehavior.CloseConnection ensures connection is closed when reader is closed
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Execute query and return DataTable
        /// </summary>
        public static DataTable ExecuteDataTable(string query, SqlParameter[] parameters = null)
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
        /// Create SQL parameter helper
        /// </summary>
        public static SqlParameter CreateParameter(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        /// <summary>
        /// Test database connection
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
    }
}
