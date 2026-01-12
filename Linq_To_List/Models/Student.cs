using System;

namespace Taska.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Course { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Phone { get; set; }

        public Student()
        {
            EnrollmentDate = DateTime.Now;
        }

        public Student(int id, string name, string email, string course, DateTime enrollmentDate, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Course = course;
            EnrollmentDate = enrollmentDate;
            Phone = phone;
        }
    }
}
