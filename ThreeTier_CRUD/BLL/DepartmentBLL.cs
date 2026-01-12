using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThreeTier_CRUD.DAL;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.BLL
{
    /// <summary>
    /// Business Logic Layer for Department operations
    /// Contains validation rules and business logic
    /// Presentation Layer should ONLY call BLL, never DAL directly
    /// </summary>
    public class DepartmentBLL
    {
        private readonly DepartmentDAL departmentDAL;

        public DepartmentBLL()
        {
            departmentDAL = new DepartmentDAL();
        }

        /// <summary>
        /// Get all departments
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            try
            {
                return departmentDAL.GetAllDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving departments: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get active departments only
        /// </summary>
        public List<Department> GetActiveDepartments()
        {
            try
            {
                return departmentDAL.GetActiveDepartments();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving active departments: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        public Department GetDepartmentById(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ValidationException("Invalid department ID");
            }

            try
            {
                Department department = departmentDAL.GetDepartmentById(departmentId);
                if (department == null)
                {
                    throw new ValidationException("Department not found");
                }
                return department;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving department: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Add new department with validation
        /// </summary>
        public int AddDepartment(Department department)
        {
            // Validate department
            ValidateDepartment(department);

            // Check for duplicates
            if (departmentDAL.DepartmentNameExists(department.DepartmentName))
            {
                throw new ValidationException("Department name already exists");
            }

            if (departmentDAL.DepartmentCodeExists(department.DepartmentCode))
            {
                throw new ValidationException("Department code already exists");
            }

            try
            {
                return departmentDAL.InsertDepartment(department);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding department: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Update department with validation
        /// </summary>
        public bool UpdateDepartment(Department department)
        {
            // Validate department ID
            if (department.DepartmentId <= 0)
            {
                throw new ValidationException("Invalid department ID");
            }

            // Validate department
            ValidateDepartment(department);

            // Check for duplicates (excluding current department)
            if (departmentDAL.DepartmentNameExists(department.DepartmentName, department.DepartmentId))
            {
                throw new ValidationException("Department name already exists");
            }

            if (departmentDAL.DepartmentCodeExists(department.DepartmentCode, department.DepartmentId))
            {
                throw new ValidationException("Department code already exists");
            }

            try
            {
                int rowsAffected = departmentDAL.UpdateDepartment(department);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating department: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete department with business rules
        /// </summary>
        public bool DeleteDepartment(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ValidationException("Invalid department ID");
            }

            // Business Rule: Cannot delete department with employees
            int employeeCount = departmentDAL.GetEmployeeCount(departmentId);
            if (employeeCount > 0)
            {
                throw new ValidationException($"Cannot delete department. It has {employeeCount} employee(s) assigned.");
            }

            try
            {
                int rowsAffected = departmentDAL.DeleteDepartment(departmentId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting department: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get employee count for department
        /// </summary>
        public int GetEmployeeCount(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ValidationException("Invalid department ID");
            }

            try
            {
                return departmentDAL.GetEmployeeCount(departmentId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting employee count: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Validate department data
        /// </summary>
        private void ValidateDepartment(Department department)
        {
            if (department == null)
            {
                throw new ValidationException("Department object cannot be null");
            }

            // Validate department name
            if (string.IsNullOrWhiteSpace(department.DepartmentName))
            {
                throw new ValidationException("Department name is required");
            }

            if (department.DepartmentName.Length < 3)
            {
                throw new ValidationException("Department name must be at least 3 characters");
            }

            if (department.DepartmentName.Length > 100)
            {
                throw new ValidationException("Department name cannot exceed 100 characters");
            }

            // Validate department code
            if (string.IsNullOrWhiteSpace(department.DepartmentCode))
            {
                throw new ValidationException("Department code is required");
            }

            if (department.DepartmentCode.Length < 2)
            {
                throw new ValidationException("Department code must be at least 2 characters");
            }

            if (department.DepartmentCode.Length > 10)
            {
                throw new ValidationException("Department code cannot exceed 10 characters");
            }

            // Business Rule: Department code should be uppercase alphanumeric
            if (!Regex.IsMatch(department.DepartmentCode, @"^[A-Z0-9]+$"))
            {
                throw new ValidationException("Department code must be uppercase alphanumeric (A-Z, 0-9)");
            }

            // Validate description (optional but with length limit)
            if (!string.IsNullOrWhiteSpace(department.Description) && department.Description.Length > 500)
            {
                throw new ValidationException("Description cannot exceed 500 characters");
            }
        }
    }
}
