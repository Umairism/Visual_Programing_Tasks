using Microsoft.EntityFrameworkCore;
using Core_MVC_EF.Models;

namespace Core_MVC_EF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Course).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(500);
                
                // Create index on Email for faster lookups
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Seed initial data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@email.com",
                    Course = "Computer Science",
                    EnrollmentDate = new DateTime(2025, 9, 1),
                    Phone = "123-456-7890",
                    Address = "123 Main St, City",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new Student
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@email.com",
                    Course = "Business Administration",
                    EnrollmentDate = new DateTime(2025, 9, 1),
                    Phone = "234-567-8901",
                    Address = "456 Oak Ave, Town",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new Student
                {
                    Id = 3,
                    Name = "Mike Johnson",
                    Email = "mike.johnson@email.com",
                    Course = "Engineering",
                    EnrollmentDate = new DateTime(2025, 9, 15),
                    Phone = "345-678-9012",
                    Address = "789 Pine Rd, Village",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}
