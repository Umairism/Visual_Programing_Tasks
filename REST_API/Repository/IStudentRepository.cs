using REST_API.Models;

namespace REST_API.Repository
{
    /// <summary>
    /// Student-specific repository interface extending the generic repository
    /// </summary>
    public interface IStudentRepository : IRepository<Student>
    {
        /// <summary>
        /// Search students by name, email, or course
        /// </summary>
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);

        /// <summary>
        /// Get students by course
        /// </summary>
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(string course);

        /// <summary>
        /// Get active students only
        /// </summary>
        Task<IEnumerable<Student>> GetActiveStudentsAsync();

        /// <summary>
        /// Get student by email
        /// </summary>
        Task<Student?> GetStudentByEmailAsync(string email);

        /// <summary>
        /// Check if email already exists
        /// </summary>
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        /// <summary>
        /// Get total count of students
        /// </summary>
        Task<int> GetTotalCountAsync();
    }
}
