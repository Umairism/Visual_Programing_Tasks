using API_Consumer.Models;

namespace API_Consumer.Services
{
    public interface IStudentApiService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(int id, Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm);
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(string course);
        Task<IEnumerable<Student>> GetActiveStudentsAsync();
        Task<int> GetTotalCountAsync();
    }
}
