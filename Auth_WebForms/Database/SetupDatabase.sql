-- ============================================
-- Authentication and Authorization Database Setup
-- Using Connectionless ADO.NET Approach
-- ============================================

USE master;
GO

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AuthWebFormsDB')
BEGIN
    CREATE DATABASE AuthWebFormsDB;
    PRINT 'Database AuthWebFormsDB created successfully.';
END
ELSE
BEGIN
    PRINT 'Database AuthWebFormsDB already exists.';
END
GO

USE AuthWebFormsDB;
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

-- ============================================
-- Stored Procedure: Authenticate User
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AuthenticateUser]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_AuthenticateUser];
GO

CREATE PROCEDURE [dbo].[sp_AuthenticateUser]
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if user exists and is not locked out
    IF EXISTS (
        SELECT 1 FROM Users 
        WHERE Username = @Username 
        AND IsActive = 1 
        AND (IsLockedOut = 0 OR LockoutEndDate < GETDATE())
    )
    BEGIN
        -- Verify password
        IF EXISTS (
            SELECT 1 FROM Users 
            WHERE Username = @Username 
            AND PasswordHash = @PasswordHash
        )
        BEGIN
            -- Successful login - Update last login date and reset failed attempts
            UPDATE Users 
            SET LastLoginDate = GETDATE(), 
                FailedLoginAttempts = 0,
                IsLockedOut = 0,
                LockoutEndDate = NULL
            WHERE Username = @Username;
            
            -- Return user details with roles
            SELECT 
                u.UserId,
                u.Username,
                u.Email,
                u.FullName,
                u.IsActive,
                u.LastLoginDate,
                STUFF((
                    SELECT ',' + r.RoleName
                    FROM UserRoles ur
                    INNER JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE ur.UserId = u.UserId
                    FOR XML PATH('')
                ), 1, 1, '') AS Roles
            FROM Users u
            WHERE u.Username = @Username;
            
            RETURN 1; -- Success
        END
        ELSE
        BEGIN
            -- Failed login - Increment failed attempts
            UPDATE Users 
            SET FailedLoginAttempts = FailedLoginAttempts + 1,
                IsLockedOut = CASE WHEN FailedLoginAttempts >= 4 THEN 1 ELSE 0 END,
                LockoutEndDate = CASE WHEN FailedLoginAttempts >= 4 THEN DATEADD(MINUTE, 30, GETDATE()) ELSE NULL END
            WHERE Username = @Username;
            
            RETURN 0; -- Invalid credentials
        END
    END
    ELSE
    BEGIN
        RETURN -1; -- User locked out or inactive
    END
END
GO

-- ============================================
-- Stored Procedure: Register User
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_RegisterUser]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_RegisterUser];
GO

CREATE PROCEDURE [dbo].[sp_RegisterUser]
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @FullName NVARCHAR(100),
    @RoleName NVARCHAR(50) = 'User'
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if username or email already exists
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
        BEGIN
            ROLLBACK TRANSACTION;
            RETURN -1; -- Username already exists
        END
        
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        BEGIN
            ROLLBACK TRANSACTION;
            RETURN -2; -- Email already exists
        END
        
        -- Insert new user
        INSERT INTO Users (Username, Email, PasswordHash, FullName, IsActive)
        VALUES (@Username, @Email, @PasswordHash, @FullName, 1);
        
        DECLARE @NewUserId INT = SCOPE_IDENTITY();
        DECLARE @RoleId INT = (SELECT RoleId FROM Roles WHERE RoleName = @RoleName);
        
        -- Assign role to user
        IF @RoleId IS NOT NULL
        BEGIN
            INSERT INTO UserRoles (UserId, RoleId) VALUES (@NewUserId, @RoleId);
        END
        
        COMMIT TRANSACTION;
        RETURN @NewUserId; -- Return new user ID
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        RETURN -99; -- Error occurred
    END CATCH
END
GO

-- ============================================
-- Stored Procedure: Get All Users (for Admin)
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllUsers]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_GetAllUsers];
GO

CREATE PROCEDURE [dbo].[sp_GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserId,
        u.Username,
        u.Email,
        u.FullName,
        u.IsActive,
        u.CreatedDate,
        u.LastLoginDate,
        u.FailedLoginAttempts,
        u.IsLockedOut,
        u.LockoutEndDate,
        STUFF((
            SELECT ',' + r.RoleName
            FROM UserRoles ur
            INNER JOIN Roles r ON ur.RoleId = r.RoleId
            WHERE ur.UserId = u.UserId
            FOR XML PATH('')
        ), 1, 1, '') AS Roles
    FROM Users u
    ORDER BY u.CreatedDate DESC;
END
GO

-- ============================================
-- Stored Procedure: Update User Status
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateUserStatus]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_UpdateUserStatus];
GO

CREATE PROCEDURE [dbo].[sp_UpdateUserStatus]
    @UserId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Users 
    SET IsActive = @IsActive,
        IsLockedOut = 0,
        LockoutEndDate = NULL,
        FailedLoginAttempts = 0
    WHERE UserId = @UserId;
    
    RETURN @@ROWCOUNT;
END
GO

-- ============================================
-- Stored Procedure: Get User Roles
-- ============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetUserRoles]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[sp_GetUserRoles];
GO

CREATE PROCEDURE [dbo].[sp_GetUserRoles]
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT r.RoleName
    FROM Users u
    INNER JOIN UserRoles ur ON u.UserId = ur.UserId
    INNER JOIN Roles r ON ur.RoleId = r.RoleId
    WHERE u.Username = @Username AND u.IsActive = 1;
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
