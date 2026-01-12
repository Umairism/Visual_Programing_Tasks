-- ============================================
-- Employee Management Database with Stored Procedures
-- ADO.NET CRUD Operations Demo
-- ============================================

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END
GO

USE EmployeeDB;
GO

-- ============================================
-- Create Tables
-- ============================================

-- Drop existing tables if they exist
IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

-- Employees Table
CREATE TABLE Employees (
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    Department NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    HireDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
GO

-- ============================================
-- Create Stored Procedures
-- ============================================

-- 1. CREATE - Insert New Employee
IF OBJECT_ID('sp_InsertEmployee', 'P') IS NOT NULL
    DROP PROCEDURE sp_InsertEmployee;
GO

CREATE PROCEDURE sp_InsertEmployee
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(256),
    @Phone NVARCHAR(20),
    @Department NVARCHAR(100),
    @Position NVARCHAR(100),
    @Salary DECIMAL(18,2),
    @HireDate DATE,
    @EmployeeId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if email already exists
        IF EXISTS (SELECT 1 FROM Employees WHERE Email = @Email)
        BEGIN
            RAISERROR('Email already exists', 16, 1);
            RETURN -1;
        END
        
        -- Insert new employee
        INSERT INTO Employees (FirstName, LastName, Email, Phone, Department, Position, Salary, HireDate, IsActive, CreatedDate)
        VALUES (@FirstName, @LastName, @Email, @Phone, @Department, @Position, @Salary, @HireDate, 1, GETDATE());
        
        -- Get the newly created EmployeeId
        SET @EmployeeId = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;
        RETURN 0;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END
GO

-- 2. READ - Get All Employees
IF OBJECT_ID('sp_GetAllEmployees', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAllEmployees;
GO

CREATE PROCEDURE sp_GetAllEmployees
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        EmployeeId,
        FirstName,
        LastName,
        Email,
        Phone,
        Department,
        Position,
        Salary,
        HireDate,
        IsActive,
        CreatedDate,
        ModifiedDate
    FROM Employees
    ORDER BY EmployeeId DESC;
END
GO

-- 3. READ - Get Active Employees Only
IF OBJECT_ID('sp_GetActiveEmployees', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetActiveEmployees;
GO

CREATE PROCEDURE sp_GetActiveEmployees
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        EmployeeId,
        FirstName,
        LastName,
        Email,
        Phone,
        Department,
        Position,
        Salary,
        HireDate,
        IsActive,
        CreatedDate,
        ModifiedDate
    FROM Employees
    WHERE IsActive = 1
    ORDER BY EmployeeId DESC;
END
GO

-- 4. READ - Get Employee By ID
IF OBJECT_ID('sp_GetEmployeeById', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeeById;
GO

CREATE PROCEDURE sp_GetEmployeeById
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        EmployeeId,
        FirstName,
        LastName,
        Email,
        Phone,
        Department,
        Position,
        Salary,
        HireDate,
        IsActive,
        CreatedDate,
        ModifiedDate
    FROM Employees
    WHERE EmployeeId = @EmployeeId;
END
GO

-- 5. UPDATE - Update Employee
IF OBJECT_ID('sp_UpdateEmployee', 'P') IS NOT NULL
    DROP PROCEDURE sp_UpdateEmployee;
GO

CREATE PROCEDURE sp_UpdateEmployee
    @EmployeeId INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(256),
    @Phone NVARCHAR(20),
    @Department NVARCHAR(100),
    @Position NVARCHAR(100),
    @Salary DECIMAL(18,2),
    @HireDate DATE,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if employee exists
        IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
        BEGIN
            RAISERROR('Employee not found', 16, 1);
            RETURN -1;
        END
        
        -- Check if email already exists for another employee
        IF EXISTS (SELECT 1 FROM Employees WHERE Email = @Email AND EmployeeId != @EmployeeId)
        BEGIN
            RAISERROR('Email already exists for another employee', 16, 1);
            RETURN -1;
        END
        
        -- Update employee
        UPDATE Employees
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            Phone = @Phone,
            Department = @Department,
            Position = @Position,
            Salary = @Salary,
            HireDate = @HireDate,
            IsActive = @IsActive,
            ModifiedDate = GETDATE()
        WHERE EmployeeId = @EmployeeId;
        
        COMMIT TRANSACTION;
        RETURN 0;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END
GO

-- 6. DELETE - Delete Employee (Hard Delete)
IF OBJECT_ID('sp_DeleteEmployee', 'P') IS NOT NULL
    DROP PROCEDURE sp_DeleteEmployee;
GO

CREATE PROCEDURE sp_DeleteEmployee
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if employee exists
        IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
        BEGIN
            RAISERROR('Employee not found', 16, 1);
            RETURN -1;
        END
        
        -- Delete employee
        DELETE FROM Employees
        WHERE EmployeeId = @EmployeeId;
        
        COMMIT TRANSACTION;
        RETURN 0;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END
GO

-- 7. UPDATE - Soft Delete (Deactivate Employee)
IF OBJECT_ID('sp_DeactivateEmployee', 'P') IS NOT NULL
    DROP PROCEDURE sp_DeactivateEmployee;
GO

CREATE PROCEDURE sp_DeactivateEmployee
    @EmployeeId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if employee exists
        IF NOT EXISTS (SELECT 1 FROM Employees WHERE EmployeeId = @EmployeeId)
        BEGIN
            RAISERROR('Employee not found', 16, 1);
            RETURN -1;
        END
        
        -- Deactivate employee
        UPDATE Employees
        SET IsActive = 0,
            ModifiedDate = GETDATE()
        WHERE EmployeeId = @EmployeeId;
        
        COMMIT TRANSACTION;
        RETURN 0;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END
GO

-- 8. READ - Search Employees
IF OBJECT_ID('sp_SearchEmployees', 'P') IS NOT NULL
    DROP PROCEDURE sp_SearchEmployees;
GO

CREATE PROCEDURE sp_SearchEmployees
    @SearchTerm NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        EmployeeId,
        FirstName,
        LastName,
        Email,
        Phone,
        Department,
        Position,
        Salary,
        HireDate,
        IsActive,
        CreatedDate,
        ModifiedDate
    FROM Employees
    WHERE 
        FirstName LIKE '%' + @SearchTerm + '%' OR
        LastName LIKE '%' + @SearchTerm + '%' OR
        Email LIKE '%' + @SearchTerm + '%' OR
        Department LIKE '%' + @SearchTerm + '%' OR
        Position LIKE '%' + @SearchTerm + '%'
    ORDER BY EmployeeId DESC;
END
GO

-- 9. READ - Get Employee Statistics
IF OBJECT_ID('sp_GetEmployeeStatistics', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeeStatistics;
GO

CREATE PROCEDURE sp_GetEmployeeStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalEmployees,
        SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS ActiveEmployees,
        SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS InactiveEmployees,
        AVG(Salary) AS AverageSalary,
        MAX(Salary) AS MaxSalary,
        MIN(Salary) AS MinSalary,
        COUNT(DISTINCT Department) AS TotalDepartments
    FROM Employees;
END
GO

-- ============================================
-- Insert Sample Data
-- ============================================

-- Insert sample employees
DECLARE @EmpId INT;

EXEC sp_InsertEmployee 'John', 'Smith', 'john.smith@company.com', '555-0101', 'IT', 'Software Engineer', 75000.00, '2020-01-15', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Jane', 'Doe', 'jane.doe@company.com', '555-0102', 'HR', 'HR Manager', 65000.00, '2019-03-20', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Michael', 'Johnson', 'michael.j@company.com', '555-0103', 'IT', 'Senior Developer', 85000.00, '2018-07-10', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Emily', 'Davis', 'emily.davis@company.com', '555-0104', 'Marketing', 'Marketing Specialist', 55000.00, '2021-05-12', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Robert', 'Wilson', 'robert.w@company.com', '555-0105', 'Finance', 'Financial Analyst', 70000.00, '2020-09-01', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Sarah', 'Brown', 'sarah.brown@company.com', '555-0106', 'IT', 'DevOps Engineer', 80000.00, '2019-11-15', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'David', 'Taylor', 'david.taylor@company.com', '555-0107', 'Sales', 'Sales Manager', 72000.00, '2021-02-28', @EmpId OUTPUT;
EXEC sp_InsertEmployee 'Lisa', 'Anderson', 'lisa.a@company.com', '555-0108', 'HR', 'Recruiter', 50000.00, '2022-01-10', @EmpId OUTPUT;

GO

-- ============================================
-- Verify Installation
-- ============================================

PRINT 'Database setup completed successfully!';
PRINT '';
PRINT 'Tables Created:';
SELECT name FROM sys.tables WHERE name = 'Employees';
PRINT '';
PRINT 'Stored Procedures Created:';
SELECT name FROM sys.procedures WHERE name LIKE 'sp_%' ORDER BY name;
PRINT '';
PRINT 'Sample Data:';
EXEC sp_GetAllEmployees;
PRINT '';
PRINT 'Employee Statistics:';
EXEC sp_GetEmployeeStatistics;
GO
