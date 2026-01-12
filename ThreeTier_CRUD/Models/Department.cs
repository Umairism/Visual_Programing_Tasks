using System;

namespace ThreeTier_CRUD.Models
{
    /// <summary>
    /// Department entity representing the Departments table
    /// </summary>
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Computed Properties
        public string Status => IsActive ? "Active" : "Inactive";
        
        public string DisplayName => $"{DepartmentCode} - {DepartmentName}";

        // Constructor
        public Department()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
    }
}
