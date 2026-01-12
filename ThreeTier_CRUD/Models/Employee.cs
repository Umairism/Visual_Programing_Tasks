using System;

namespace ThreeTier_CRUD.Models
{
    /// <summary>
    /// Employee entity representing the Employees table
    /// </summary>
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties (for display purposes)
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }

        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";
        
        public string Status => IsActive ? "Active" : "Inactive";
        
        public string FormattedSalary => Salary.ToString("C");
        
        public string FormattedHireDate => HireDate.ToString("MMM dd, yyyy");

        public int YearsOfService => (DateTime.Now - HireDate).Days / 365;

        // Constructor
        public Employee()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
            HireDate = DateTime.Now;
        }
    }
}
