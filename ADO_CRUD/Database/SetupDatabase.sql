-- ============================================
-- ADO.NET CRUD Database Setup
-- Pure ADO.NET approach with direct database access
-- ============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'StudentDB')
BEGIN
    ALTER DATABASE StudentDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE StudentDB;
END
GO

-- Create database
CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

-- ============================================
-- CREATE TABLES
-- ============================================

-- Courses Table
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY IDENTITY(1,1),
    CourseCode NVARCHAR(20) NOT NULL UNIQUE,
    CourseName NVARCHAR(200) NOT NULL,
    Credits INT NOT NULL,
    Department NVARCHAR(100),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Students Table
CREATE TABLE Students (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    StudentNumber NVARCHAR(20) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    DateOfBirth DATE NOT NULL,
    CourseId INT NOT NULL,
    EnrollmentDate DATE DEFAULT GETDATE(),
    GPA DECIMAL(3,2) DEFAULT 0.00,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Students_Courses FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

-- ============================================
-- INSERT SAMPLE DATA
-- ============================================

-- Insert Courses
INSERT INTO Courses (CourseCode, CourseName, Credits, Department, IsActive) VALUES
('CS101', 'Introduction to Computer Science', 3, 'Computer Science', 1),
('CS201', 'Data Structures and Algorithms', 4, 'Computer Science', 1),
('CS301', 'Database Management Systems', 4, 'Computer Science', 1),
('CS401', 'Software Engineering', 3, 'Computer Science', 1),
('IT101', 'Information Technology Fundamentals', 3, 'Information Technology', 1),
('IT201', 'Network Administration', 4, 'Information Technology', 1),
('BUS101', 'Business Administration', 3, 'Business', 1),
('ENG101', 'English Composition', 3, 'English', 1);

-- Insert Students
INSERT INTO Students (StudentNumber, FirstName, LastName, Email, DateOfBirth, CourseId, EnrollmentDate, GPA, IsActive) VALUES
('S2024001', 'John', 'Smith', 'john.smith@university.edu', '2001-05-15', 1, '2024-09-01', 3.75, 1),
('S2024002', 'Emma', 'Johnson', 'emma.johnson@university.edu', '2002-03-22', 2, '2024-09-01', 3.85, 1),
('S2024003', 'Michael', 'Williams', 'michael.williams@university.edu', '2001-11-08', 1, '2024-09-01', 3.50, 1),
('S2024004', 'Sarah', 'Brown', 'sarah.brown@university.edu', '2002-07-30', 3, '2024-09-01', 3.92, 1),
('S2024005', 'David', 'Jones', 'david.jones@university.edu', '2001-09-14', 4, '2024-09-01', 3.45, 1),
('S2024006', 'Emily', 'Davis', 'emily.davis@university.edu', '2002-01-25', 5, '2024-09-01', 3.68, 1),
('S2024007', 'Daniel', 'Miller', 'daniel.miller@university.edu', '2001-12-03', 2, '2024-09-01', 3.55, 1),
('S2024008', 'Olivia', 'Wilson', 'olivia.wilson@university.edu', '2002-04-18', 6, '2024-09-01', 3.88, 1),
('S2024009', 'James', 'Moore', 'james.moore@university.edu', '2001-08-27', 1, '2024-09-01', 3.72, 1),
('S2024010', 'Sophia', 'Taylor', 'sophia.taylor@university.edu', '2002-06-11', 3, '2024-09-01', 3.95, 1),
('S2024011', 'William', 'Anderson', 'william.anderson@university.edu', '2001-10-20', 7, '2024-09-01', 3.40, 1),
('S2024012', 'Ava', 'Thomas', 'ava.thomas@university.edu', '2002-02-09', 8, '2024-09-01', 3.78, 1);

-- ============================================
-- CREATE VIEWS
-- ============================================

-- Student Summary View
CREATE VIEW vw_StudentSummary AS
SELECT 
    s.StudentId,
    s.StudentNumber,
    s.FirstName + ' ' + s.LastName AS FullName,
    s.Email,
    s.DateOfBirth,
    DATEDIFF(YEAR, s.DateOfBirth, GETDATE()) AS Age,
    c.CourseName,
    c.CourseCode,
    s.GPA,
    s.EnrollmentDate,
    DATEDIFF(DAY, s.EnrollmentDate, GETDATE()) AS DaysEnrolled,
    s.IsActive,
    CASE 
        WHEN s.GPA >= 3.75 THEN 'Excellent'
        WHEN s.GPA >= 3.50 THEN 'Very Good'
        WHEN s.GPA >= 3.00 THEN 'Good'
        WHEN s.GPA >= 2.50 THEN 'Satisfactory'
        ELSE 'Needs Improvement'
    END AS PerformanceStatus
FROM Students s
INNER JOIN Courses c ON s.CourseId = c.CourseId;

-- Statistics View
CREATE VIEW vw_Statistics AS
SELECT 
    (SELECT COUNT(*) FROM Students WHERE IsActive = 1) AS TotalStudents,
    (SELECT COUNT(*) FROM Courses WHERE IsActive = 1) AS TotalCourses,
    (SELECT AVG(GPA) FROM Students WHERE IsActive = 1) AS AverageGPA,
    (SELECT COUNT(*) FROM Students WHERE GPA >= 3.75 AND IsActive = 1) AS ExcellentStudents,
    (SELECT TOP 1 CourseName FROM Courses c 
     INNER JOIN Students s ON c.CourseId = s.CourseId 
     WHERE s.IsActive = 1 
     GROUP BY c.CourseName 
     ORDER BY COUNT(*) DESC) AS MostPopularCourse;

-- ============================================
-- CREATE STORED PROCEDURES
-- ============================================

-- Get Student by ID
CREATE PROCEDURE sp_GetStudentById
    @StudentId INT
AS
BEGIN
    SELECT s.*, c.CourseName, c.CourseCode
    FROM Students s
    INNER JOIN Courses c ON s.CourseId = c.CourseId
    WHERE s.StudentId = @StudentId;
END
GO

-- Search Students
CREATE PROCEDURE sp_SearchStudents
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
           s.Email, s.GPA, c.CourseName, s.IsActive
    FROM Students s
    INNER JOIN Courses c ON s.CourseId = c.CourseId
    WHERE s.FirstName LIKE '%' + @SearchTerm + '%'
       OR s.LastName LIKE '%' + @SearchTerm + '%'
       OR s.Email LIKE '%' + @SearchTerm + '%'
       OR s.StudentNumber LIKE '%' + @SearchTerm + '%'
    ORDER BY s.StudentId DESC;
END
GO

-- ============================================
-- VERIFICATION
-- ============================================

PRINT '========================================';
PRINT 'Database Setup Complete!';
PRINT '========================================';
PRINT 'Total Courses: ' + CAST((SELECT COUNT(*) FROM Courses) AS NVARCHAR(10));
PRINT 'Total Students: ' + CAST((SELECT COUNT(*) FROM Students) AS NVARCHAR(10));
PRINT 'Average GPA: ' + CAST((SELECT AVG(GPA) FROM Students) AS NVARCHAR(10));
PRINT '========================================';

-- Show sample data
SELECT 'Sample Students' AS Info;
SELECT TOP 5 StudentNumber, FirstName, LastName, CourseName, GPA 
FROM vw_StudentSummary 
ORDER BY GPA DESC;
