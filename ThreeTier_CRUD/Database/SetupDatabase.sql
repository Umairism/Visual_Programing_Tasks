-- ============================================
-- 3-Tier Architecture Database Setup
-- Database: ThreeTierDB
-- ============================================

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ThreeTierDB')
BEGIN
    CREATE DATABASE ThreeTierDB;
END
GO

USE ThreeTierDB;
GO

-- ============================================
-- Drop existing tables if they exist
-- ============================================
IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

IF OBJECT_ID('Departments', 'U') IS NOT NULL
    DROP TABLE Departments;
GO

-- ============================================
-- Create Departments Table
-- ============================================
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    DepartmentCode NVARCHAR(10) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

-- ============================================
-- Create Employees Table
-- ============================================
CREATE TABLE Employees (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    DepartmentId INT NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    HireDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Employee_Department FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);
GO

-- ============================================
-- Create Indexes for Performance
-- ============================================
CREATE NONCLUSTERED INDEX IX_Employees_DepartmentId ON Employees(DepartmentId);
CREATE NONCLUSTERED INDEX IX_Employees_Email ON Employees(Email);
CREATE NONCLUSTERED INDEX IX_Employees_IsActive ON Employees(IsActive);
GO

-- ============================================
-- Insert Sample Departments
-- ============================================
INSERT INTO Departments (DepartmentName, DepartmentCode, Description) VALUES
('Information Technology', 'IT', 'Responsible for technology infrastructure and software development'),
('Human Resources', 'HR', 'Manages employee relations, recruitment, and benefits'),
('Finance', 'FIN', 'Handles financial operations, accounting, and budgeting'),
('Marketing', 'MKT', 'Develops marketing strategies and brand management'),
('Sales', 'SAL', 'Manages sales operations and customer relationships'),
('Operations', 'OPS', 'Oversees daily business operations and processes');
GO

-- ============================================
-- Insert Sample Employees
-- ============================================
DECLARE @ITDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'IT');
DECLARE @HRDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'HR');
DECLARE @FinDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'FIN');
DECLARE @MktDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'MKT');
DECLARE @SalDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'SAL');
DECLARE @OpsDeptId INT = (SELECT DepartmentId FROM Departments WHERE DepartmentCode = 'OPS');

INSERT INTO Employees (FirstName, LastName, Email, Phone, DepartmentId, Position, Salary, HireDate) VALUES
('John', 'Smith', 'john.smith@company.com', '555-0101', @ITDeptId, 'Software Engineer', 75000.00, '2023-01-15'),
('Sarah', 'Johnson', 'sarah.johnson@company.com', '555-0102', @ITDeptId, 'Senior Developer', 95000.00, '2022-06-01'),
('Michael', 'Williams', 'michael.williams@company.com', '555-0103', @HRDeptId, 'HR Manager', 70000.00, '2021-03-20'),
('Emily', 'Brown', 'emily.brown@company.com', '555-0104', @FinDeptId, 'Financial Analyst', 68000.00, '2023-09-10'),
('David', 'Jones', 'david.jones@company.com', '555-0105', @MktDeptId, 'Marketing Specialist', 62000.00, '2023-02-14'),
('Jessica', 'Garcia', 'jessica.garcia@company.com', '555-0106', @SalDeptId, 'Sales Manager', 85000.00, '2022-11-05'),
('Daniel', 'Martinez', 'daniel.martinez@company.com', '555-0107', @OpsDeptId, 'Operations Coordinator', 58000.00, '2023-07-18'),
('Lisa', 'Rodriguez', 'lisa.rodriguez@company.com', '555-0108', @ITDeptId, 'DevOps Engineer', 88000.00, '2022-08-22'),
('James', 'Wilson', 'james.wilson@company.com', '555-0109', @FinDeptId, 'Accountant', 65000.00, '2023-04-30'),
('Maria', 'Lopez', 'maria.lopez@company.com', '555-0110', @HRDeptId, 'Recruiter', 55000.00, '2023-10-12');
GO

-- ============================================
-- Verify Data
-- ============================================
SELECT 'Departments' AS TableName, COUNT(*) AS RecordCount FROM Departments
UNION ALL
SELECT 'Employees' AS TableName, COUNT(*) AS RecordCount FROM Employees;
GO

SELECT 
    d.DepartmentName,
    COUNT(e.EmployeeId) AS EmployeeCount,
    AVG(e.Salary) AS AverageSalary
FROM Departments d
LEFT JOIN Employees e ON d.DepartmentId = e.DepartmentId
GROUP BY d.DepartmentName
ORDER BY EmployeeCount DESC;
GO

PRINT 'Database setup completed successfully!';
