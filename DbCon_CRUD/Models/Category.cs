using System;

namespace DbCon_CRUD.Models
{
    /// <summary>
    /// Category entity model
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // Computed properties
        public string Status => IsActive ? "Active" : "Inactive";

        public Category()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
    }
}
