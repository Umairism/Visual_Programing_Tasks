-- ============================================
-- Centralized DbCon CRUD Database Setup
-- Database: InventoryDB
-- ============================================

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InventoryDB')
BEGIN
    CREATE DATABASE InventoryDB;
END
GO

USE InventoryDB;
GO

-- ============================================
-- Drop existing tables if they exist
-- ============================================
IF OBJECT_ID('Products', 'U') IS NOT NULL
    DROP TABLE Products;
GO

IF OBJECT_ID('Categories', 'U') IS NOT NULL
    DROP TABLE Categories;
GO

-- ============================================
-- Create Categories Table
-- ============================================
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================
-- Create Products Table
-- ============================================
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(200) NOT NULL,
    CategoryId INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    Description NVARCHAR(1000),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
GO

-- ============================================
-- Create Indexes
-- ============================================
CREATE NONCLUSTERED INDEX IX_Products_CategoryId ON Products(CategoryId);
CREATE NONCLUSTERED INDEX IX_Products_IsActive ON Products(IsActive);
GO

-- ============================================
-- Insert Sample Categories
-- ============================================
INSERT INTO Categories (CategoryName, Description) VALUES
('Electronics', 'Electronic devices and accessories'),
('Computers', 'Desktop computers, laptops, and accessories'),
('Software', 'Operating systems and application software'),
('Networking', 'Routers, switches, and network equipment'),
('Storage', 'Hard drives, SSDs, and storage devices'),
('Peripherals', 'Keyboards, mice, monitors, and other peripherals');
GO

-- ============================================
-- Insert Sample Products
-- ============================================
DECLARE @ElectronicsId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Electronics');
DECLARE @ComputersId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Computers');
DECLARE @SoftwareId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Software');
DECLARE @NetworkingId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Networking');
DECLARE @StorageId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Storage');
DECLARE @PeripheralsId INT = (SELECT CategoryId FROM Categories WHERE CategoryName = 'Peripherals');

INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description) VALUES
('Dell Laptop XPS 15', @ComputersId, 1499.99, 25, '15.6" laptop with Intel Core i7 processor and 16GB RAM'),
('HP Desktop ProDesk 600', @ComputersId, 899.99, 15, 'Desktop computer with Intel Core i5 and 8GB RAM'),
('Logitech Wireless Mouse', @PeripheralsId, 29.99, 150, 'Ergonomic wireless mouse with USB receiver'),
('Samsung 27" Monitor', @PeripheralsId, 299.99, 45, '27-inch Full HD LED monitor with 5ms response time'),
('Seagate 2TB External HDD', @StorageId, 79.99, 80, 'Portable external hard drive with USB 3.0'),
('Kingston 512GB SSD', @StorageId, 119.99, 60, 'Internal solid-state drive for faster performance'),
('Windows 11 Pro', @SoftwareId, 199.99, 100, 'Microsoft Windows 11 Professional operating system'),
('Microsoft Office 365', @SoftwareId, 99.99, 200, 'Office productivity suite with annual subscription'),
('Cisco Gigabit Router', @NetworkingId, 149.99, 35, '5-port gigabit ethernet router'),
('TP-Link WiFi 6 Router', @NetworkingId, 89.99, 50, 'Dual-band wireless router with WiFi 6 support'),
('Mechanical Keyboard RGB', @PeripheralsId, 79.99, 70, 'Gaming mechanical keyboard with RGB backlighting'),
('Webcam HD 1080p', @ElectronicsId, 49.99, 90, 'Full HD webcam with built-in microphone'),
('USB-C Hub 7-in-1', @ElectronicsId, 39.99, 120, 'Multi-port USB-C hub with HDMI and card reader'),
('Laptop Cooling Pad', @ComputersId, 24.99, 65, 'Adjustable laptop cooling pad with dual fans'),
('Cable Management Kit', @ElectronicsId, 14.99, 200, 'Complete cable management solution for desks');
GO

-- ============================================
-- Create View for Product Statistics
-- ============================================
CREATE VIEW vw_ProductStatistics AS
SELECT 
    COUNT(*) AS TotalProducts,
    COUNT(CASE WHEN IsActive = 1 THEN 1 END) AS ActiveProducts,
    COUNT(CASE WHEN IsActive = 0 THEN 1 END) AS InactiveProducts,
    SUM(StockQuantity) AS TotalStock,
    AVG(Price) AS AveragePrice,
    MIN(Price) AS MinPrice,
    MAX(Price) AS MaxPrice,
    SUM(Price * StockQuantity) AS TotalInventoryValue
FROM Products;
GO

-- ============================================
-- Verify Data
-- ============================================
SELECT 'Categories' AS TableName, COUNT(*) AS RecordCount FROM Categories
UNION ALL
SELECT 'Products' AS TableName, COUNT(*) AS RecordCount FROM Products;
GO

SELECT 
    c.CategoryName,
    COUNT(p.ProductId) AS ProductCount,
    SUM(p.StockQuantity) AS TotalStock,
    AVG(p.Price) AS AveragePrice
FROM Categories c
LEFT JOIN Products p ON c.CategoryId = p.CategoryId
GROUP BY c.CategoryName
ORDER BY ProductCount DESC;
GO

SELECT * FROM vw_ProductStatistics;
GO

PRINT 'Database setup completed successfully!';
PRINT 'Sample data: 6 categories and 15 products created.';
