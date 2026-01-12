using System.ComponentModel.DataAnnotations;

namespace REST_API.DTOs
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required")]
        [StringLength(100)]
        public string Course { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enrollment date is required")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone format")]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
