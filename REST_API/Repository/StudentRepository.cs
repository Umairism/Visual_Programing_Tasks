using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Models;

namespace REST_API.Repository
{
    /// <summary>
    /// Concrete implementation of Student Repository with full CRUD operations
    /// </summary>
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Get all students
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // GET: Get student by ID
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // POST: Add new student
        public async Task<Student> AddAsync(Student entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            await _context.Students.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        // PUT: Update existing student
        public async Task<Student> UpdateAsync(Student entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _context.Students.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        // DELETE: Delete student by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await GetByIdAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            return await SaveChangesAsync();
        }

        // Check if student exists
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(s => s.Id == id);
        }

        // Save changes to database
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        // SEARCH: Search students by name, email, or course
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Students
                .Where(s => s.Name.Contains(searchTerm) ||
                           s.Email.Contains(searchTerm) ||
                           s.Course.Contains(searchTerm))
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // GET: Get students by course
        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(string course)
        {
            return await _context.Students
                .Where(s => s.Course.ToLower() == course.ToLower())
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // GET: Get active students only
        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // GET: Get student by email
        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Email.ToLower() == email.ToLower());
        }

        // Check if email already exists (excluding current student for updates)
        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _context.Students.Where(s => s.Email.ToLower() == email.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        // GET: Get total count of students
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Students.CountAsync();
        }
    }
}
