using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.DAL
{
    /// <summary>
    /// Data Access Layer for Employee operations
    /// PURE DATABASE OPERATIONS - NO BUSINESS LOGIC
    /// </summary>
    public class EmployeeDAL
    {
        /// <summary>
        /// Get all employees with department information
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Phone, 
                            e.DepartmentId, d.DepartmentName, d.DepartmentCode, e.Position, 
                            e.Salary, e.HireDate, e.IsActive, e.CreatedDate, e.ModifiedDate
                            FROM Employees e
                            INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                            ORDER BY e.EmployeeId DESC";

            using (SqlDataReader reader = DBHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    employees.Add(MapReaderToEmployee(reader));
                }
            }

            return employees;
        }

        /// <summary>
        /// Get active employees only
        /// </summary>
        public List<Employee> GetActiveEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Phone, 
                            e.DepartmentId, d.DepartmentName, d.DepartmentCode, e.Position, 
                            e.Salary, e.HireDate, e.IsActive, e.CreatedDate, e.ModifiedDate
                            FROM Employees e
                            INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                            WHERE e.IsActive = 1
                            ORDER BY e.EmployeeId DESC";

            using (SqlDataReader reader = DBHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    employees.Add(MapReaderToEmployee(reader));
                }
            }

            return employees;
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int employeeId)
        {
            string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Phone, 
                            e.DepartmentId, d.DepartmentName, d.DepartmentCode, e.Position, 
                            e.Salary, e.HireDate, e.IsActive, e.CreatedDate, e.ModifiedDate
                            FROM Employees e
                            INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                            WHERE e.EmployeeId = @EmployeeId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@EmployeeId", employeeId)
            };

            using (SqlDataReader reader = DBHelper.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return MapReaderToEmployee(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Get employees by department
        /// </summary>
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            List<Employee> employees = new List<Employee>();
            string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Phone, 
                            e.DepartmentId, d.DepartmentName, d.DepartmentCode, e.Position, 
                            e.Salary, e.HireDate, e.IsActive, e.CreatedDate, e.ModifiedDate
                            FROM Employees e
                            INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                            WHERE e.DepartmentId = @DepartmentId
                            ORDER BY e.LastName, e.FirstName";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentId", departmentId)
            };

            using (SqlDataReader reader = DBHelper.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    employees.Add(MapReaderToEmployee(reader));
                }
            }

            return employees;
        }

        /// <summary>
        /// Search employees by keyword
        /// </summary>
        public List<Employee> SearchEmployees(string keyword)
        {
            List<Employee> employees = new List<Employee>();
            string query = @"SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Phone, 
                            e.DepartmentId, d.DepartmentName, d.DepartmentCode, e.Position, 
                            e.Salary, e.HireDate, e.IsActive, e.CreatedDate, e.ModifiedDate
                            FROM Employees e
                            INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                            WHERE e.FirstName LIKE @Keyword 
                               OR e.LastName LIKE @Keyword 
                               OR e.Email LIKE @Keyword 
                               OR e.Position LIKE @Keyword
                               OR d.DepartmentName LIKE @Keyword
                            ORDER BY e.LastName, e.FirstName";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@Keyword", "%" + keyword + "%")
            };

            using (SqlDataReader reader = DBHelper.ExecuteReader(query, parameters))
            {
                while (reader.Read())
                {
                    employees.Add(MapReaderToEmployee(reader));
                }
            }

            return employees;
        }

        /// <summary>
        /// Check if email exists
        /// </summary>
        public bool EmailExists(string email, int excludeEmployeeId = 0)
        {
            string query = @"SELECT COUNT(*) FROM Employees 
                            WHERE Email = @Email 
                            AND EmployeeId != @ExcludeEmployeeId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@Email", email),
                DBHelper.CreateParameter("@ExcludeEmployeeId", excludeEmployeeId)
            };

            int count = Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        /// <summary>
        /// Insert new employee
        /// </summary>
        public int InsertEmployee(Employee employee)
        {
            string query = @"INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, 
                            Position, Salary, HireDate, IsActive, CreatedDate)
                            VALUES (@FirstName, @LastName, @Email, @Phone, @DepartmentId, 
                            @Position, @Salary, @HireDate, @IsActive, GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@FirstName", employee.FirstName),
                DBHelper.CreateParameter("@LastName", employee.LastName),
                DBHelper.CreateParameter("@Email", employee.Email),
                DBHelper.CreateParameter("@Phone", employee.Phone),
                DBHelper.CreateParameter("@DepartmentId", employee.DepartmentId),
                DBHelper.CreateParameter("@Position", employee.Position),
                DBHelper.CreateParameter("@Salary", employee.Salary),
                DBHelper.CreateParameter("@HireDate", employee.HireDate),
                DBHelper.CreateParameter("@IsActive", employee.IsActive)
            };

            return Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
        }

        /// <summary>
        /// Update employee
        /// </summary>
        public int UpdateEmployee(Employee employee)
        {
            string query = @"UPDATE Employees 
                            SET FirstName = @FirstName,
                                LastName = @LastName,
                                Email = @Email,
                                Phone = @Phone,
                                DepartmentId = @DepartmentId,
                                Position = @Position,
                                Salary = @Salary,
                                HireDate = @HireDate,
                                IsActive = @IsActive,
                                ModifiedDate = GETDATE()
                            WHERE EmployeeId = @EmployeeId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@EmployeeId", employee.EmployeeId),
                DBHelper.CreateParameter("@FirstName", employee.FirstName),
                DBHelper.CreateParameter("@LastName", employee.LastName),
                DBHelper.CreateParameter("@Email", employee.Email),
                DBHelper.CreateParameter("@Phone", employee.Phone),
                DBHelper.CreateParameter("@DepartmentId", employee.DepartmentId),
                DBHelper.CreateParameter("@Position", employee.Position),
                DBHelper.CreateParameter("@Salary", employee.Salary),
                DBHelper.CreateParameter("@HireDate", employee.HireDate),
                DBHelper.CreateParameter("@IsActive", employee.IsActive)
            };

            return DBHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        public int DeleteEmployee(int employeeId)
        {
            string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@EmployeeId", employeeId)
            };

            return DBHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        public Dictionary<string, object> GetEmployeeStatistics()
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();
            string query = @"SELECT 
                            COUNT(*) AS TotalEmployees,
                            COUNT(CASE WHEN IsActive = 1 THEN 1 END) AS ActiveEmployees,
                            COUNT(CASE WHEN IsActive = 0 THEN 1 END) AS InactiveEmployees,
                            AVG(Salary) AS AverageSalary,
                            MIN(Salary) AS MinSalary,
                            MAX(Salary) AS MaxSalary
                            FROM Employees";

            using (SqlDataReader reader = DBHelper.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    stats["TotalEmployees"] = reader["TotalEmployees"] == DBNull.Value ? 0 : (int)reader["TotalEmployees"];
                    stats["ActiveEmployees"] = reader["ActiveEmployees"] == DBNull.Value ? 0 : (int)reader["ActiveEmployees"];
                    stats["InactiveEmployees"] = reader["InactiveEmployees"] == DBNull.Value ? 0 : (int)reader["InactiveEmployees"];
                    stats["AverageSalary"] = reader["AverageSalary"] == DBNull.Value ? 0m : (decimal)reader["AverageSalary"];
                    stats["MinSalary"] = reader["MinSalary"] == DBNull.Value ? 0m : (decimal)reader["MinSalary"];
                    stats["MaxSalary"] = reader["MaxSalary"] == DBNull.Value ? 0m : (decimal)reader["MaxSalary"];
                }
            }

            return stats;
        }

        /// <summary>
        /// Map SqlDataReader to Employee object
        /// </summary>
        private Employee MapReaderToEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = (int)reader["EmployeeId"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                DepartmentId = (int)reader["DepartmentId"],
                DepartmentName = reader["DepartmentName"].ToString(),
                DepartmentCode = reader["DepartmentCode"].ToString(),
                Position = reader["Position"].ToString(),
                Salary = (decimal)reader["Salary"],
                HireDate = (DateTime)reader["HireDate"],
                IsActive = (bool)reader["IsActive"],
                CreatedDate = (DateTime)reader["CreatedDate"],
                ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : (DateTime?)reader["ModifiedDate"]
            };
        }
    }
}
