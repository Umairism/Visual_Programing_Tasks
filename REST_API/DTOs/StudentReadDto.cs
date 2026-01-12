namespace REST_API.DTOs
{
    public class StudentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
