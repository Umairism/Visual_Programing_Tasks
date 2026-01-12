# Centralized DbCon CRUD Application

A comprehensive ASP.NET Web Forms application demonstrating the **Centralized Database Connection Pattern** using a single static utility class (`DbCon`) for all database operations.

## 🏗️ Architecture Overview

This project uses a **simplified 2-tier architecture** where the Presentation Layer calls the DbCon utility class directly:

```
┌─────────────────────────────────────┐
│      Presentation Layer             │
│  (Web Forms - .aspx files)          │
│  - ProductList.aspx                 │
│  - ProductAdd.aspx                  │
│  - ProductEdit.aspx                 │
│  - CategoryList.aspx                │
└─────────────┬───────────────────────┘
              │ calls directly
              ↓
┌─────────────────────────────────────┐
│      DbCon Utility Class            │
│  (Static Class - Utilities/DbCon.cs)│
│  - ExecuteNonQuery()                │
│  - ExecuteScalar()                  │
│  - ExecuteReader()                  │
│  - ExecuteDataTable()               │
│  - CreateParameter()                │
└─────────────┬───────────────────────┘
              │ executes SQL
              ↓
┌─────────────────────────────────────┐
│      SQL Server Database            │
│  (InventoryDB)                      │
│  - Products Table                   │
│  - Categories Table                 │
└─────────────────────────────────────┘
```

## ✨ Key Features

### 1. **Centralized DbCon Class**
- Single static class for all database operations
- No instantiation needed - all methods are static
- Simplifies database access across the application
- Located in `Utilities/DbCon.cs`

### 2. **Core Database Methods**

```csharp
// Execute INSERT, UPDATE, DELETE - returns rows affected
int rowsAffected = DbCon.ExecuteNonQuery(query, parameters);

// Get single value (COUNT, MAX, SUM, etc.)
object result = DbCon.ExecuteScalar(query, parameters);
int count = Convert.ToInt32(DbCon.ExecuteScalar("SELECT COUNT(*) FROM Products"));

// Read data with SqlDataReader
using (SqlDataReader reader = DbCon.ExecuteReader(query, parameters))
{
    while (reader.Read())
    {
        // Process rows
    }
}

// Get DataTable for GridView binding
DataTable dt = DbCon.ExecuteDataTable(query, parameters);
gvProducts.DataSource = dt;
gvProducts.DataBind();

// Get DataSet with multiple tables
DataSet ds = DbCon.ExecuteDataSet(query, parameters);
```

### 3. **Helper Methods**

```csharp
// Create SQL parameters safely
SqlParameter param = DbCon.CreateParameter("@Name", "Product Name");
SqlParameter param2 = DbCon.CreateParameter("@Price", 99.99m);

// Create OUTPUT parameters
SqlParameter outputParam = DbCon.CreateOutputParameter("@NewId", SqlDbType.Int);

// Check if record exists
bool exists = DbCon.RecordExists("Products", "ProductName", "Laptop");

// Get record count
int count = DbCon.GetRecordCount("Products", "WHERE IsActive = 1");

// Verify table existence
bool tableExists = DbCon.TableExists("Products");

// Execute multiple commands in a transaction
DbCon.ExecuteTransaction((transaction) => {
    DbCon.ExecuteNonQuery(query1, parameters1, transaction);
    DbCon.ExecuteNonQuery(query2, parameters2, transaction);
});
```

## 📊 Database Schema

### Products Table
```sql
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(200) NOT NULL,
    CategoryId INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL,
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
```

### Categories Table
```sql
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

### vw_ProductStatistics View
```sql
CREATE VIEW vw_ProductStatistics AS
SELECT 
    COUNT(*) AS TotalProducts,
    COUNT(CASE WHEN IsActive = 1 THEN 1 END) AS ActiveProducts,
    SUM(StockQuantity) AS TotalStock,
    AVG(Price) AS AveragePrice,
    SUM(Price * StockQuantity) AS TotalInventoryValue
FROM Products;
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- SQL Server LocalDB or SQL Server

### Installation

1. **Setup Database**
```powershell
# Open SQL Server Management Studio and run:
Database/SetupDatabase.sql
```

2. **Update Connection String**
```xml
<!-- Web.config -->
<connectionStrings>
    <add name="InventoryDBConnection" 
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\InventoryDB.mdf;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

3. **Build and Run**
- Open `DbCon_CRUD.csproj` in Visual Studio
- Build solution (Ctrl + Shift + B)
- Run application (F5)

## 💻 Code Examples

### Example 1: Loading Products with Search
```csharp
protected void btnSearch_Click(object sender, EventArgs e)
{
    string keyword = txtSearch.Text.Trim();
    
    string query = @"SELECT p.*, c.CategoryName 
                    FROM Products p
                    INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                    WHERE p.ProductName LIKE @Keyword
                    ORDER BY p.ProductId DESC";
    
    SqlParameter[] parameters = {
        DbCon.CreateParameter("@Keyword", "%" + keyword + "%")
    };
    
    DataTable dt = DbCon.ExecuteDataTable(query, parameters);
    gvProducts.DataSource = dt;
    gvProducts.DataBind();
}
```

### Example 2: Adding a Product
```csharp
protected void btnSave_Click(object sender, EventArgs e)
{
    string query = @"INSERT INTO Products (ProductName, CategoryId, Price, StockQuantity, Description, IsActive, CreatedDate)
                    VALUES (@ProductName, @CategoryId, @Price, @StockQuantity, @Description, @IsActive, GETDATE());
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
    
    SqlParameter[] parameters = {
        DbCon.CreateParameter("@ProductName", txtProductName.Text),
        DbCon.CreateParameter("@CategoryId", ddlCategory.SelectedValue),
        DbCon.CreateParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
        DbCon.CreateParameter("@StockQuantity", Convert.ToInt32(txtStock.Text)),
        DbCon.CreateParameter("@Description", txtDescription.Text),
        DbCon.CreateParameter("@IsActive", chkIsActive.Checked)
    };
    
    int newId = Convert.ToInt32(DbCon.ExecuteScalar(query, parameters));
    Response.Redirect($"ProductList.aspx?success=Product added! (ID: {newId})");
}
```

### Example 3: Updating a Product
```csharp
protected void btnUpdate_Click(object sender, EventArgs e)
{
    string query = @"UPDATE Products 
                    SET ProductName = @ProductName,
                        CategoryId = @CategoryId,
                        Price = @Price,
                        StockQuantity = @StockQuantity,
                        ModifiedDate = GETDATE()
                    WHERE ProductId = @ProductId";
    
    SqlParameter[] parameters = {
        DbCon.CreateParameter("@ProductId", Convert.ToInt32(txtProductId.Text)),
        DbCon.CreateParameter("@ProductName", txtProductName.Text),
        DbCon.CreateParameter("@CategoryId", ddlCategory.SelectedValue),
        DbCon.CreateParameter("@Price", Convert.ToDecimal(txtPrice.Text)),
        DbCon.CreateParameter("@StockQuantity", Convert.ToInt32(txtStock.Text))
    };
    
    int rowsAffected = DbCon.ExecuteNonQuery(query, parameters);
    if (rowsAffected > 0)
        Response.Redirect("ProductList.aspx?success=Product updated!");
}
```

### Example 4: Deleting a Product
```csharp
protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "DeleteProduct")
    {
        int productId = Convert.ToInt32(e.CommandArgument);
        
        string query = "DELETE FROM Products WHERE ProductId = @ProductId";
        SqlParameter[] parameters = {
            DbCon.CreateParameter("@ProductId", productId)
        };
        
        DbCon.ExecuteNonQuery(query, parameters);
        LoadProducts();
    }
}
```

### Example 5: Getting Statistics
```csharp
private void LoadStatistics()
{
    string query = "SELECT * FROM vw_ProductStatistics";
    DataTable dt = DbCon.ExecuteDataTable(query);
    
    if (dt.Rows.Count > 0)
    {
        DataRow row = dt.Rows[0];
        lblTotalProducts.Text = row["TotalProducts"].ToString();
        lblTotalStock.Text = row["TotalStock"].ToString();
        lblAvgPrice.Text = Convert.ToDecimal(row["AveragePrice"]).ToString("C");
        lblInventoryValue.Text = Convert.ToDecimal(row["TotalInventoryValue"]).ToString("C");
    }
}
```

## 📁 Project Structure

```
DbCon_CRUD/
├── Database/
│   └── SetupDatabase.sql          # Database creation script
├── Utilities/
│   └── DbCon.cs                   # Centralized database class
├── Models/
│   ├── Product.cs                 # Product entity
│   └── Category.cs                # Category entity
├── Products/
│   ├── ProductList.aspx           # List all products
│   ├── ProductList.aspx.cs
│   ├── ProductAdd.aspx            # Add new product
│   ├── ProductAdd.aspx.cs
│   ├── ProductEdit.aspx           # Edit product
│   └── ProductEdit.aspx.cs
├── Categories/
│   ├── CategoryList.aspx          # List categories
│   └── CategoryList.aspx.cs
├── Styles/
│   └── site.css                   # Application styles
├── Default.aspx                   # Home page
├── Default.aspx.cs
├── Web.config                     # Configuration
└── README.md
```

## ⚖️ Centralized DbCon vs 3-Tier Architecture

### Centralized DbCon Pattern (This Project)

**✅ Advantages:**
- **Simplicity**: Easy to understand and implement
- **Speed**: Faster development for small projects
- **Direct Access**: No intermediate layers to navigate
- **Less Code**: Fewer files and classes to manage
- **Single Point**: All database logic in one place
- **Quick Prototyping**: Ideal for demos and MVPs

**❌ Disadvantages:**
- **Limited Separation**: Business logic mixed with presentation
- **Testing Challenges**: Harder to unit test
- **Scalability Issues**: Difficult to scale for large applications
- **Code Duplication**: Validation may repeat across pages
- **Team Collaboration**: All developers touch the same class
- **Tight Coupling**: Presentation tightly coupled to database

**Best For:**
- Small to medium applications (< 50 pages)
- Internal tools and utilities
- Rapid prototyping and MVPs
- Learning ADO.NET
- Single developer projects
- Simple CRUD operations

### 3-Tier Architecture

**✅ Advantages:**
- **Clear Separation**: Distinct layers (Presentation → BLL → DAL)
- **Testability**: Each layer can be unit tested
- **Scalability**: Easy to add features and scale
- **Reusability**: Business logic reused across multiple UIs
- **Maintainability**: Changes isolated to specific layers
- **Team Collaboration**: Different teams work on different layers

**❌ Disadvantages:**
- **Complexity**: More files and classes
- **Learning Curve**: Steeper for beginners
- **Development Time**: Slower initial development
- **Over-engineering**: May be overkill for simple apps

**Best For:**
- Enterprise applications
- Multiple presentation layers (Web, Desktop, Mobile)
- Complex business logic
- Team development
- Long-term maintenance
- Applications requiring extensive testing

## 🔄 When to Use Each Pattern

| Factor | DbCon Pattern | 3-Tier Architecture |
|--------|--------------|---------------------|
| **Project Size** | Small/Medium | Medium/Large |
| **Team Size** | 1-2 developers | 3+ developers |
| **Timeline** | Short (days/weeks) | Long (months/years) |
| **Business Logic** | Simple | Complex |
| **Testing Requirements** | Minimal | Extensive |
| **Future Scalability** | Limited | High |
| **Budget** | Low | Medium/High |

## 🛡️ Security Features

1. **Parameterized Queries**: All queries use SQL parameters to prevent SQL injection
2. **Input Validation**: ASP.NET validators on all forms
3. **Connection Management**: Automatic connection disposal using `using` statements
4. **Error Handling**: Try-catch blocks with user-friendly messages
5. **Type Safety**: Strongly-typed parameter creation

## 🎯 Best Practices Implemented

1. **Use Static Methods**: No need to instantiate DbCon class
2. **CommandBehavior.CloseConnection**: Ensures connections close automatically
3. **Using Statements**: Proper resource disposal
4. **Parameter Helpers**: Simplify parameter creation
5. **ExecuteDataTable for GridView**: Easy data binding
6. **Transaction Support**: For multi-command operations
7. **Connection String in Web.config**: Centralized configuration

## 🧪 Testing the Application

1. **Add Products**: Navigate to Products → Add Product
2. **Search Products**: Use search box on Product List
3. **Filter by Category**: Use category dropdown
4. **Edit Products**: Click edit icon on any product
5. **Delete Products**: Click delete icon (confirms first)
6. **View Statistics**: Dashboard shows real-time stats
7. **Manage Categories**: View and manage product categories

## 📈 Sample Data

The database includes:
- **6 Categories**: Electronics, Computers, Software, Networking, Storage, Peripherals
- **15 Products**: Including laptops, monitors, keyboards, mice, SSDs, etc.
- **Price Range**: $14.99 to $1,499.99
- **Stock Levels**: 0 to 500 units

## 🔧 DbCon Class Methods Reference

| Method | Purpose | Returns | Example |
|--------|---------|---------|---------|
| `ExecuteNonQuery` | INSERT, UPDATE, DELETE | `int` (rows affected) | `DbCon.ExecuteNonQuery(sql, params)` |
| `ExecuteScalar` | Single value queries | `object` | `DbCon.ExecuteScalar("SELECT COUNT(*)")` |
| `ExecuteReader` | SELECT queries | `SqlDataReader` | `DbCon.ExecuteReader(sql, params)` |
| `ExecuteDataTable` | SELECT to DataTable | `DataTable` | `DbCon.ExecuteDataTable(sql, params)` |
| `ExecuteDataSet` | Multiple result sets | `DataSet` | `DbCon.ExecuteDataSet(sql, params)` |
| `CreateParameter` | Create SQL parameter | `SqlParameter` | `DbCon.CreateParameter("@Id", 1)` |
| `RecordExists` | Check record existence | `bool` | `DbCon.RecordExists("Products", "Id", 1)` |
| `GetRecordCount` | Count records | `int` | `DbCon.GetRecordCount("Products")` |
| `TableExists` | Verify table | `bool` | `DbCon.TableExists("Products")` |
| `TestConnection` | Test connection | `bool` | `DbCon.TestConnection()` |
| `ExecuteTransaction` | Run transaction | `void` | `DbCon.ExecuteTransaction(action)` |

## 🌟 Learning Outcomes

By studying this project, you'll learn:
1. How to create a centralized database utility class
2. When to use DbCon pattern vs 3-tier architecture
3. ADO.NET fundamentals (SqlConnection, SqlCommand, SqlDataReader)
4. Parameterized queries and SQL injection prevention
5. Transaction management
6. GridView data binding with DataTable
7. CRUD operations in ASP.NET Web Forms
8. Connection string management
9. Error handling and resource disposal
10. Simple but effective architecture for small projects

## 📚 Related Projects

- **ThreeTier_CRUD**: Employee management using 3-tier architecture with strict layer separation
- **StoredProcedure_CRUD**: CRUD operations using stored procedures
- **EF_Core_CRUD**: Entity Framework Core approach (ORM)

## 🤝 Contributing

This is an educational project demonstrating the centralized DbCon pattern. Feel free to:
- Add new features (e.g., export to Excel)
- Improve error handling
- Add more DbCon utility methods
- Enhance UI/UX
- Add unit tests

## 📝 License

This project is for educational purposes.

## 👨‍💻 Author

Created as part of Visual Programming practice series demonstrating different architectural patterns in ASP.NET.

---

**Note**: This project demonstrates a **simplified approach** suitable for small applications. For enterprise applications with complex business logic, consider using **3-Tier Architecture** or **Clean Architecture** patterns instead.

