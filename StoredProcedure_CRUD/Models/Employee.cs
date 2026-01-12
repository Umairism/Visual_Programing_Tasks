using System;

namespace StoredProcedure_CRUD.Models
{
    /// <summary>
    /// Employee Model Class
    /// Represents an employee entity
    /// </summary>
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Computed Properties
        public string FullName => $"{FirstName} {LastName}";
        public string Status => IsActive ? "Active" : "Inactive";
        public string FormattedSalary => Salary.ToString("C");
        public string FormattedHireDate => HireDate.ToString("MMM dd, yyyy");
    }
}
