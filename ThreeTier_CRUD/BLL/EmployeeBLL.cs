using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThreeTier_CRUD.DAL;
using ThreeTier_CRUD.Models;

namespace ThreeTier_CRUD.BLL
{
    /// <summary>
    /// Business Logic Layer for Employee operations
    /// Contains validation rules and business logic
    /// Presentation Layer should ONLY call BLL, never DAL directly
    /// </summary>
    public class EmployeeBLL
    {
        private readonly EmployeeDAL employeeDAL;
        private readonly DepartmentDAL departmentDAL;

        public EmployeeBLL()
        {
            employeeDAL = new EmployeeDAL();
            departmentDAL = new DepartmentDAL();
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        public List<Employee> GetAllEmployees()
        {
            try
            {
                return employeeDAL.GetAllEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employees: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get active employees only
        /// </summary>
        public List<Employee> GetActiveEmployees()
        {
            try
            {
                return employeeDAL.GetActiveEmployees();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving active employees: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        public Employee GetEmployeeById(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ValidationException("Invalid employee ID");
            }

            try
            {
                Employee employee = employeeDAL.GetEmployeeById(employeeId);
                if (employee == null)
                {
                    throw new ValidationException("Employee not found");
                }
                return employee;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employee: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get employees by department
        /// </summary>
        public List<Employee> GetEmployeesByDepartment(int departmentId)
        {
            if (departmentId <= 0)
            {
                throw new ValidationException("Invalid department ID");
            }

            try
            {
                return employeeDAL.GetEmployeesByDepartment(departmentId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving employees by department: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Search employees by keyword
        /// </summary>
        public List<Employee> SearchEmployees(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAllEmployees();
            }

            try
            {
                return employeeDAL.SearchEmployees(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching employees: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Add new employee with validation
        /// </summary>
        public int AddEmployee(Employee employee)
        {
            // Validate employee
            ValidateEmployee(employee);

            // Business Rule: Department must be active
            Department department = departmentDAL.GetDepartmentById(employee.DepartmentId);
            if (department == null)
            {
                throw new ValidationException("Selected department does not exist");
            }
            if (!department.IsActive)
            {
                throw new ValidationException("Cannot assign employee to an inactive department");
            }

            // Check for duplicate email
            if (employeeDAL.EmailExists(employee.Email))
            {
                throw new ValidationException("Email already exists");
            }

            try
            {
                return employeeDAL.InsertEmployee(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Update employee with validation
        /// </summary>
        public bool UpdateEmployee(Employee employee)
        {
            // Validate employee ID
            if (employee.EmployeeId <= 0)
            {
                throw new ValidationException("Invalid employee ID");
            }

            // Validate employee
            ValidateEmployee(employee);

            // Business Rule: Department must exist and be active (only for active employees)
            Department department = departmentDAL.GetDepartmentById(employee.DepartmentId);
            if (department == null)
            {
                throw new ValidationException("Selected department does not exist");
            }
            if (!department.IsActive && employee.IsActive)
            {
                throw new ValidationException("Cannot assign active employee to an inactive department");
            }

            // Check for duplicate email (excluding current employee)
            if (employeeDAL.EmailExists(employee.Email, employee.EmployeeId))
            {
                throw new ValidationException("Email already exists");
            }

            try
            {
                int rowsAffected = employeeDAL.UpdateEmployee(employee);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating employee: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        public bool DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ValidationException("Invalid employee ID");
            }

            try
            {
                int rowsAffected = employeeDAL.DeleteEmployee(employeeId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting employee: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Get employee statistics
        /// </summary>
        public Dictionary<string, object> GetEmployeeStatistics()
        {
            try
            {
                return employeeDAL.GetEmployeeStatistics();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving statistics: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Validate employee data
        /// </summary>
        private void ValidateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ValidationException("Employee object cannot be null");
            }

            // Validate first name
            if (string.IsNullOrWhiteSpace(employee.FirstName))
            {
                throw new ValidationException("First name is required");
            }
            if (employee.FirstName.Length < 2)
            {
                throw new ValidationException("First name must be at least 2 characters");
            }
            if (employee.FirstName.Length > 50)
            {
                throw new ValidationException("First name cannot exceed 50 characters");
            }
            if (!Regex.IsMatch(employee.FirstName, @"^[a-zA-Z\s'-]+$"))
            {
                throw new ValidationException("First name can only contain letters, spaces, hyphens, and apostrophes");
            }

            // Validate last name
            if (string.IsNullOrWhiteSpace(employee.LastName))
            {
                throw new ValidationException("Last name is required");
            }
            if (employee.LastName.Length < 2)
            {
                throw new ValidationException("Last name must be at least 2 characters");
            }
            if (employee.LastName.Length > 50)
            {
                throw new ValidationException("Last name cannot exceed 50 characters");
            }
            if (!Regex.IsMatch(employee.LastName, @"^[a-zA-Z\s'-]+$"))
            {
                throw new ValidationException("Last name can only contain letters, spaces, hyphens, and apostrophes");
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(employee.Email))
            {
                throw new ValidationException("Email is required");
            }
            if (!Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                throw new ValidationException("Invalid email format");
            }
            if (employee.Email.Length > 100)
            {
                throw new ValidationException("Email cannot exceed 100 characters");
            }

            // Validate phone (optional but with format if provided)
            if (!string.IsNullOrWhiteSpace(employee.Phone))
            {
                if (!Regex.IsMatch(employee.Phone, @"^[\d\s\-\+\(\)]+$"))
                {
                    throw new ValidationException("Invalid phone format. Only digits, spaces, hyphens, plus signs, and parentheses allowed");
                }
                if (employee.Phone.Length > 20)
                {
                    throw new ValidationException("Phone cannot exceed 20 characters");
                }
            }

            // Validate department
            if (employee.DepartmentId <= 0)
            {
                throw new ValidationException("Department is required");
            }

            // Validate position
            if (string.IsNullOrWhiteSpace(employee.Position))
            {
                throw new ValidationException("Position is required");
            }
            if (employee.Position.Length < 3)
            {
                throw new ValidationException("Position must be at least 3 characters");
            }
            if (employee.Position.Length > 100)
            {
                throw new ValidationException("Position cannot exceed 100 characters");
            }

            // Validate salary
            if (employee.Salary <= 0)
            {
                throw new ValidationException("Salary must be greater than zero");
            }
            if (employee.Salary < 1000)
            {
                throw new ValidationException("Salary must be at least $1,000");
            }
            if (employee.Salary > 1000000)
            {
                throw new ValidationException("Salary cannot exceed $1,000,000");
            }

            // Validate hire date
            if (employee.HireDate == DateTime.MinValue)
            {
                throw new ValidationException("Hire date is required");
            }
            
            // Business Rule: Hire date cannot be in the future
            if (employee.HireDate > DateTime.Now.Date)
            {
                throw new ValidationException("Hire date cannot be in the future");
            }
            
            // Business Rule: Hire date cannot be more than 50 years in the past
            if (employee.HireDate < DateTime.Now.AddYears(-50))
            {
                throw new ValidationException("Hire date cannot be more than 50 years in the past");
            }
        }
    }
}
