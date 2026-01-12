using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taska.Models;

namespace Taska.DataAccess
{
    public class StudentRepository
    {
        // Static list to store student data (simulating a database)
        private static List<Student> students = new List<Student>();
        private static int nextId = 1;

        static StudentRepository()
        {
            // Initialize with some sample data
            students.Add(new Student(nextId++, "John Doe", "john.doe@email.com", "Computer Science", DateTime.Now.AddDays(-30), "123-456-7890"));
            students.Add(new Student(nextId++, "Jane Smith", "jane.smith@email.com", "Business Administration", DateTime.Now.AddDays(-45), "234-567-8901"));
            students.Add(new Student(nextId++, "Mike Johnson", "mike.johnson@email.com", "Engineering", DateTime.Now.AddDays(-60), "345-678-9012"));
        }

        // READ - Get all students using LINQ
        public List<Student> GetAllStudents()
        {
            return students.OrderBy(s => s.Name).ToList();
        }

        // READ - Get student by ID using LINQ
        public Student GetStudentById(int id)
        {
            return students.FirstOrDefault(s => s.Id == id);
        }

        // CREATE - Add new student using LINQ
        public bool AddStudent(Student student)
        {
            try
            {
                student.Id = nextId++;
                students.Add(student);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // UPDATE - Update existing student using LINQ
        public bool UpdateStudent(Student updatedStudent)
        {
            try
            {
                var existingStudent = students.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (existingStudent != null)
                {
                    existingStudent.Name = updatedStudent.Name;
                    existingStudent.Email = updatedStudent.Email;
                    existingStudent.Course = updatedStudent.Course;
                    existingStudent.EnrollmentDate = updatedStudent.EnrollmentDate;
                    existingStudent.Phone = updatedStudent.Phone;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // DELETE - Remove student using LINQ
        public bool DeleteStudent(int id)
        {
            try
            {
                var student = students.FirstOrDefault(s => s.Id == id);
                if (student != null)
                {
                    students.Remove(student);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // SEARCH - Find students by name or email using LINQ
        public List<Student> SearchStudents(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return GetAllStudents();

            return students
                .Where(s => s.Name.Contains(searchTerm) || s.Email.Contains(searchTerm) || s.Course.Contains(searchTerm))
                .OrderBy(s => s.Name)
                .ToList();
        }

        // Get count of students using LINQ
        public int GetStudentCount()
        {
            return students.Count();
        }

        // Get students by course using LINQ
        public List<Student> GetStudentsByCourse(string course)
        {
            return students
                .Where(s => s.Course.Equals(course, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Name)
                .ToList();
        }
    }
}
