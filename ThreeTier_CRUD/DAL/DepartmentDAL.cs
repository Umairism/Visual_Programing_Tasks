using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.DAL
{
    /// <summary>
    /// Data Access Layer for Department operations
    /// PURE DATABASE OPERATIONS - NO BUSINESS LOGIC
    /// </summary>
    public class DepartmentDAL
    {
        /// <summary>
        /// Get all departments
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = @"SELECT DepartmentId, DepartmentName, DepartmentCode, Description, 
                            IsActive, CreatedDate, ModifiedDate 
                            FROM Departments 
                            ORDER BY DepartmentName";

            using (SqlDataReader reader = DBHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    departments.Add(MapReaderToDepartment(reader));
                }
            }

            return departments;
        }

        /// <summary>
        /// Get active departments only
        /// </summary>
        public List<Department> GetActiveDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = @"SELECT DepartmentId, DepartmentName, DepartmentCode, Description, 
                            IsActive, CreatedDate, ModifiedDate 
                            FROM Departments 
                            WHERE IsActive = 1
                            ORDER BY DepartmentName";

            using (SqlDataReader reader = DBHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    departments.Add(MapReaderToDepartment(reader));
                }
            }

            return departments;
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        public Department GetDepartmentById(int departmentId)
        {
            string query = @"SELECT DepartmentId, DepartmentName, DepartmentCode, Description, 
                            IsActive, CreatedDate, ModifiedDate 
                            FROM Departments 
                            WHERE DepartmentId = @DepartmentId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentId", departmentId)
            };

            using (SqlDataReader reader = DBHelper.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return MapReaderToDepartment(reader);
                }
            }

            return null;
        }

        /// <summary>
        /// Check if department name exists
        /// </summary>
        public bool DepartmentNameExists(string departmentName, int excludeDepartmentId = 0)
        {
            string query = @"SELECT COUNT(*) FROM Departments 
                            WHERE DepartmentName = @DepartmentName 
                            AND DepartmentId != @ExcludeDepartmentId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentName", departmentName),
                DBHelper.CreateParameter("@ExcludeDepartmentId", excludeDepartmentId)
            };

            int count = Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        /// <summary>
        /// Check if department code exists
        /// </summary>
        public bool DepartmentCodeExists(string departmentCode, int excludeDepartmentId = 0)
        {
            string query = @"SELECT COUNT(*) FROM Departments 
                            WHERE DepartmentCode = @DepartmentCode 
                            AND DepartmentId != @ExcludeDepartmentId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentCode", departmentCode),
                DBHelper.CreateParameter("@ExcludeDepartmentId", excludeDepartmentId)
            };

            int count = Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        /// <summary>
        /// Insert new department
        /// </summary>
        public int InsertDepartment(Department department)
        {
            string query = @"INSERT INTO Departments (DepartmentName, DepartmentCode, Description, IsActive, CreatedDate)
                            VALUES (@DepartmentName, @DepartmentCode, @Description, @IsActive, GETDATE());
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentName", department.DepartmentName),
                DBHelper.CreateParameter("@DepartmentCode", department.DepartmentCode),
                DBHelper.CreateParameter("@Description", department.Description),
                DBHelper.CreateParameter("@IsActive", department.IsActive)
            };

            return Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
        }

        /// <summary>
        /// Update department
        /// </summary>
        public int UpdateDepartment(Department department)
        {
            string query = @"UPDATE Departments 
                            SET DepartmentName = @DepartmentName,
                                DepartmentCode = @DepartmentCode,
                                Description = @Description,
                                IsActive = @IsActive,
                                ModifiedDate = GETDATE()
                            WHERE DepartmentId = @DepartmentId";

            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentId", department.DepartmentId),
                DBHelper.CreateParameter("@DepartmentName", department.DepartmentName),
                DBHelper.CreateParameter("@DepartmentCode", department.DepartmentCode),
                DBHelper.CreateParameter("@Description", department.Description),
                DBHelper.CreateParameter("@IsActive", department.IsActive)
            };

            return DBHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Delete department
        /// </summary>
        public int DeleteDepartment(int departmentId)
        {
            string query = "DELETE FROM Departments WHERE DepartmentId = @DepartmentId";
            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentId", departmentId)
            };

            return DBHelper.ExecuteNonQuery(query, parameters);
        }

        /// <summary>
        /// Get employee count for department
        /// </summary>
        public int GetEmployeeCount(int departmentId)
        {
            string query = "SELECT COUNT(*) FROM Employees WHERE DepartmentId = @DepartmentId";
            SqlParameter[] parameters = {
                DBHelper.CreateParameter("@DepartmentId", departmentId)
            };

            return Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
        }

        /// <summary>
        /// Map SqlDataReader to Department object
        /// </summary>
        private Department MapReaderToDepartment(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentId = (int)reader["DepartmentId"],
                DepartmentName = reader["DepartmentName"].ToString(),
                DepartmentCode = reader["DepartmentCode"].ToString(),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                IsActive = (bool)reader["IsActive"],
                CreatedDate = (DateTime)reader["CreatedDate"],
                ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? null : (DateTime?)reader["ModifiedDate"]
            };
        }
    }
}
