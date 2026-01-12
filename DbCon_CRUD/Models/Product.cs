using System;

namespace DbCon_CRUD.Models
{
    /// <summary>
    /// Product entity model
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public string CategoryName { get; set; }

        // Computed properties
        public string Status => IsActive ? "Active" : "Inactive";
        
        public string FormattedPrice => Price.ToString("C");
        
        public string StockStatus
        {
            get
            {
                if (StockQuantity == 0) return "Out of Stock";
                if (StockQuantity < 10) return "Low Stock";
                return "In Stock";
            }
        }

        public string TotalValue => (Price * StockQuantity).ToString("C");

        public Product()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
            StockQuantity = 0;
        }
    }
}
