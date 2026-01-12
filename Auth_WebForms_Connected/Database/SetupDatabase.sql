-- ============================================
-- Authentication and Authorization Database Setup
-- Using Connection-Oriented ADO.NET Approach
-- ============================================

USE master;
GO

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AuthWebFormsConnectedDB')
BEGIN
    CREATE DATABASE AuthWebFormsConnectedDB;
    PRINT 'Database AuthWebFormsConnectedDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database AuthWebFormsConnectedDB already exists.';
END
GO

USE AuthWebFormsConnectedDB;
GO

-- ============================================
-- Table: Roles
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Roles]
    (
        RoleId INT PRIMARY KEY IDENTITY(1,1),
        RoleName NVARCHAR(50) NOT NULL UNIQUE,
        Description NVARCHAR(200) NULL,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table Roles created successfully.';
END
GO

-- ============================================
-- Table: Users
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users]
    (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        LastLoginDate DATETIME NULL,
        FailedLoginAttempts INT DEFAULT 0,
        IsLockedOut BIT DEFAULT 0,
        LockoutEndDate DATETIME NULL
    );
    PRINT 'Table Users created successfully.';
END
GO

-- ============================================
-- Table: UserRoles (Many-to-Many Relationship)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserRoles]
    (
        UserRoleId INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        RoleId INT NOT NULL,
        AssignedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE,
        UNIQUE(UserId, RoleId)
    );
    PRINT 'Table UserRoles created successfully.';
END
GO

-- ============================================
-- Insert Default Roles
-- ============================================
IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'Admin')
BEGIN
    INSERT INTO Roles (RoleName, Description) VALUES ('Admin', 'Administrator with full access');
    PRINT 'Admin role inserted.';
END

IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'User')
BEGIN
    INSERT INTO Roles (RoleName, Description) VALUES ('User', 'Regular user with limited access');
    PRINT 'User role inserted.';
END

IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'Guest')
BEGIN
    INSERT INTO Roles (RoleName, Description) VALUES ('Guest', 'Guest user with read-only access');
    PRINT 'Guest role inserted.';
END
GO

-- ============================================
-- Insert Sample Users
-- Password: "Admin@123" (hashed with SHA256)
-- ============================================
DECLARE @AdminPasswordHash NVARCHAR(255) = 'BA3253876AED6BC22D4A6FF53D8406C6AD864195ED144AB5C87621B6C233B548BAEAE6956DF346EC8C17F5EA10F35EE3CBC514797ED7DDD3145464E2A0BAB413';
DECLARE @UserPasswordHash NVARCHAR(255) = '3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2';

-- Insert Admin User (if not exists)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FullName, IsActive)
    VALUES ('admin', 'admin@example.com', @AdminPasswordHash, 'System Administrator', 1);
    
    DECLARE @AdminUserId INT = SCOPE_IDENTITY();
    DECLARE @AdminRoleId INT = (SELECT RoleId FROM Roles WHERE RoleName = 'Admin');
    
    INSERT INTO UserRoles (UserId, RoleId) VALUES (@AdminUserId, @AdminRoleId);
    PRINT 'Admin user created with Admin role.';
END
GO

-- Insert Regular User (if not exists)
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'john.doe')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FullName, IsActive)
    VALUES ('john.doe', 'john.doe@example.com', '3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2', 'John Doe', 1);
    
    DECLARE @UserId INT = SCOPE_IDENTITY();
    DECLARE @UserRoleId INT = (SELECT RoleId FROM Roles WHERE RoleName = 'User');
    
    INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @UserRoleId);
    PRINT 'Regular user created with User role.';
END
GO

PRINT '============================================';
PRINT 'Database setup completed successfully!';
PRINT '============================================';
PRINT 'Default Credentials:';
PRINT 'Admin - Username: admin, Password: Admin@123';
PRINT 'User - Username: john.doe, Password: User@123';
PRINT '============================================';
GO
