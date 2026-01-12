using System.ComponentModel.DataAnnotations;

namespace REST_API.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Course { get; set; } = string.Empty;

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}
