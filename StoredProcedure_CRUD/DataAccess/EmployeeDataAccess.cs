using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using StoredProcedure_CRUD.Models;

namespace StoredProcedure_CRUD.DataAccess
{
    /// <summary>
    /// Employee Data Access Layer
    /// Uses ADO.NET with Stored Procedures for CRUD operations
    /// </summary>
    public class EmployeeDataAccess
    {
        private readonly string connectionString;

        public EmployeeDataAccess()
        {
            connectionString = ConfigurationManager.ConnectionStrings["EmployeeDBConnection"].ConnectionString;
        }

        #region CREATE - Insert Employee

        /// <summary>
        /// Insert a new employee using stored procedure
        /// </summary>
        public int InsertEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_InsertEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@Phone", (object)employee.Phone ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Department", employee.Department);
                        command.Parameters.AddWithValue("@Position", employee.Position);
                        command.Parameters.AddWithValue("@Salary", employee.Salary);
                        command.Parameters.AddWithValue("@HireDate", employee.HireDate);

                        // Output parameter
                        SqlParameter outputParam = new SqlParameter("@EmployeeId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Return value parameter
                        SqlParameter returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Check return value
                        int returnValue = (int)returnParam.Value;
                        if (returnValue == 0)
                        {
                            return (int)outputParam.Value;
                        }
                        else
                        {
                            throw new Exception("Failed to insert employee");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting employee: {ex.Message}", ex);
            }
        }

        #endregion

        #region READ - Get Employees

        /// <summary>
        /// Get all employees using stored procedure
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetAllEmployees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapReaderToEmployee(reader));
                            }
                        }
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employees: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get active employees only using stored procedure
        /// </summary>
        public List<Employee> GetActiveEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetActiveEmployees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapReaderToEmployee(reader));
                            }
                        }
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving active employees: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get employee by ID using stored procedure
        /// </summary>
        public Employee GetEmployeeById(int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetEmployeeById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapReaderToEmployee(reader);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employee: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Search employees using stored procedure
        /// </summary>
        public List<Employee> SearchEmployees(string searchTerm)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_SearchEmployees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", searchTerm);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employees.Add(MapReaderToEmployee(reader));
                            }
                        }
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching employees: {ex.Message}", ex);
            }
        }

        #endregion

        #region UPDATE - Update Employee

        /// <summary>
        /// Update employee using stored procedure
        /// </summary>
        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_UpdateEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@Email", employee.Email);
                        command.Parameters.AddWithValue("@Phone", (object)employee.Phone ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Department", employee.Department);
                        command.Parameters.AddWithValue("@Position", employee.Position);
                        command.Parameters.AddWithValue("@Salary", employee.Salary);
                        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
                        command.Parameters.AddWithValue("@IsActive", employee.IsActive);

                        // Return value parameter
                        SqlParameter returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Check return value
                        int returnValue = (int)returnParam.Value;
                        return returnValue == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating employee: {ex.Message}", ex);
            }
        }

        #endregion

        #region DELETE - Delete Employee

        /// <summary>
        /// Delete employee using stored procedure (hard delete)
        /// </summary>
        public bool DeleteEmployee(int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeleteEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);

                        // Return value parameter
                        SqlParameter returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Check return value
                        int returnValue = (int)returnParam.Value;
                        return returnValue == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting employee: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deactivate employee using stored procedure (soft delete)
        /// </summary>
        public bool DeactivateEmployee(int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_DeactivateEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmployeeId", employeeId);

                        // Return value parameter
                        SqlParameter returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };
                        command.Parameters.Add(returnParam);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Check return value
                        int returnValue = (int)returnParam.Value;
                        return returnValue == 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating employee: {ex.Message}", ex);
            }
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Get employee statistics using stored procedure
        /// </summary>
        public Dictionary<string, object> GetEmployeeStatistics()
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("sp_GetEmployeeStatistics", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats["TotalEmployees"] = reader["TotalEmployees"];
                                stats["ActiveEmployees"] = reader["ActiveEmployees"];
                                stats["InactiveEmployees"] = reader["InactiveEmployees"];
                                stats["AverageSalary"] = reader["AverageSalary"];
                                stats["MaxSalary"] = reader["MaxSalary"];
                                stats["MinSalary"] = reader["MinSalary"];
                                stats["TotalDepartments"] = reader["TotalDepartments"];
                            }
                        }
                    }
                }

                return stats;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving statistics: {ex.Message}", ex);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Map SqlDataReader to Employee object
        /// </summary>
        private Employee MapReaderToEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Department = reader.GetString(reader.GetOrdinal("Department")),
                Position = reader.GetString(reader.GetOrdinal("Position")),
                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                ModifiedDate = reader.IsDBNull(reader.GetOrdinal("ModifiedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
            };
        }

        #endregion
    }
}
