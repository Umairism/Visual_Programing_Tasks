using Microsoft.EntityFrameworkCore;
using REST_API.Models;

namespace REST_API.Data
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
                
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Address)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Create unique index on Email
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Student_Email");

                // Create index on Course for faster filtering
                entity.HasIndex(e => e.Course)
                    .HasDatabaseName("IX_Student_Course");
            });

            // Seed initial data
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    Name = "Alice Johnson",
                    Email = "alice.johnson@email.com",
                    Course = "Computer Science",
                    EnrollmentDate = new DateTime(2025, 9, 1),
                    Phone = "111-222-3333",
                    Address = "123 Tech Street, Silicon Valley",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Student
                {
                    Id = 2,
                    Name = "Bob Williams",
                    Email = "bob.williams@email.com",
                    Course = "Data Science",
                    EnrollmentDate = new DateTime(2025, 9, 1),
                    Phone = "222-333-4444",
                    Address = "456 Data Avenue, Boston",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Student
                {
                    Id = 3,
                    Name = "Carol Martinez",
                    Email = "carol.martinez@email.com",
                    Course = "Software Engineering",
                    EnrollmentDate = new DateTime(2025, 9, 15),
                    Phone = "333-444-5555",
                    Address = "789 Code Lane, Seattle",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new Student
                {
                    Id = 4,
                    Name = "David Brown",
                    Email = "david.brown@email.com",
                    Course = "Artificial Intelligence",
                    EnrollmentDate = new DateTime(2025, 10, 1),
                    Phone = "444-555-6666",
                    Address = "321 AI Boulevard, San Francisco",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            );
        }
    }
}
