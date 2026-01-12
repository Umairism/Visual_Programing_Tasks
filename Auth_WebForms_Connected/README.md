# ASP.NET Web Forms Authentication & Authorization - Connection-Oriented Approach

## 🔌 Connection-Oriented ADO.NET Architecture

This project demonstrates **authentication and authorization** using the **connection-oriented (connected)** approach in ADO.NET with ASP.NET Web Forms. It showcases manual connection management, SqlDataReader for data streaming, and direct database interaction.

---

## 📊 Connection-Oriented vs Connectionless Comparison

| Feature | Connection-Oriented (This Project) | Connectionless (Disconnected) |
|---------|-----------------------------------|-------------------------------|
| **Classes Used** | SqlConnection, SqlCommand, SqlDataReader | SqlDataAdapter, DataSet, DataTable |
| **Connection** | Manual Open/Close | Automatic management |
| **Data Access** | Forward-only, read-only stream | In-memory data cache |
| **Performance** | Better for large datasets | Better for small datasets |
| **Memory Usage** | Lower (streaming) | Higher (full dataset in memory) |
| **Use Case** | Real-time data, streaming | Offline operations, data binding |
| **Code Complexity** | More manual control | Simpler with abstraction |

---

## 🏗️ Architecture Overview

### Connection-Oriented Components

1. **SqlConnection**
   - Represents a connection to SQL Server
   - Must be manually opened and closed
   - Uses connection pooling for efficiency

2. **SqlCommand**
   - Executes SQL statements against database
   - Methods: ExecuteReader(), ExecuteNonQuery(), ExecuteScalar()

3. **SqlDataReader**
   - Forward-only, read-only data stream
   - Fastest way to retrieve data
   - Must be closed after use

---

## 🗂️ Project Structure

```
Auth_WebForms_Connected/
│
├── Database/
│   └── SetupDatabase.sql          # Database schema and sample data
│
├── DataAccess/
│   └── UserDataAccess.cs          # Connection-oriented data access layer
│
├── Helpers/
│   └── SecurityHelper.cs          # Password hashing and validation
│
├── Styles/
│   └── site.css                   # Connection-oriented themed CSS
│
├── Login.aspx / Login.aspx.cs     # User authentication page
├── Register.aspx / Register.aspx.cs # User registration page
├── Logout.aspx / Logout.aspx.cs   # User logout functionality
├── Default.aspx / Default.aspx.cs # Home page
├── AdminPanel.aspx / AdminPanel.aspx.cs # Admin dashboard
├── UserDashboard.aspx / UserDashboard.aspx.cs # User dashboard
├── Global.asax / Global.asax.cs   # Application events and role handling
└── Web.config                     # Configuration and security settings
```

---

## 💾 Database Schema

```sql
-- Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(256) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsLocked BIT NOT NULL DEFAULT 0,
    FailedLoginAttempts INT NOT NULL DEFAULT 0,
    LastLoginDate DATETIME NULL,
    LockoutEndDate DATETIME NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

-- UserRoles Table (Many-to-Many)
CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
);
```

---

## 🔧 Setup Instructions

### 1. Database Setup

1. Open **SQL Server Management Studio (SSMS)**
2. Execute the SQL script from `Database/SetupDatabase.sql`
3. Verify tables and sample data are created

### 2. Connection String Configuration

Update the connection string in `Web.config`:

```xml
<connectionStrings>
  <add name="AuthDbConnection" 
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AuthDB_Connected;Integrated Security=True" 
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

### 3. Build and Run

1. Open the project in **Visual Studio**
2. Build the solution (Ctrl + Shift + B)
3. Run the application (F5)

---

## 🔐 Default Credentials

| Username | Password | Role | Status |
|----------|----------|------|--------|
| admin | Admin@123 | Admin | Active |
| john.doe | User@123 | User | Active |

---

## 🚀 Key Features

### Connection-Oriented Data Access

#### 1. **Manual Connection Management**

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    // Execute commands
    
    connection.Close(); // Explicitly close
}
```

#### 2. **SqlDataReader for Data Retrieval**

```csharp
public UserInfo AuthenticateUser(string username, string password)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = @"SELECT u.UserId, u.Username, u.Email, u.FullName, 
                         u.IsActive, u.IsLocked, u.FailedLoginAttempts, 
                         u.LockoutEndDate, r.RoleName
                         FROM Users u
                         INNER JOIN UserRoles ur ON u.UserId = ur.UserId
                         INNER JOIN Roles r ON ur.RoleId = r.RoleId
                         WHERE u.Username = @Username";

        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Username", username);

        connection.Open();
        
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader.Read())
            {
                return new UserInfo
                {
                    UserId = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Email = reader.GetString(2),
                    FullName = reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    IsLocked = reader.GetBoolean(5),
                    FailedLoginAttempts = reader.GetInt32(6),
                    LockoutEndDate = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                    Role = reader.GetString(8)
                };
            }
        }
    }
    return null;
}
```

#### 3. **ExecuteNonQuery for Inserts/Updates**

```csharp
public bool UpdateUserStatus(int userId, bool isActive)
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = "UPDATE Users SET IsActive = @IsActive WHERE UserId = @UserId";
        
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@IsActive", isActive);
        command.Parameters.AddWithValue("@UserId", userId);
        
        connection.Open();
        int rowsAffected = command.ExecuteNonQuery();
        
        return rowsAffected > 0;
    }
}
```

#### 4. **ExecuteScalar for Single Values**

```csharp
public int GetUserCount()
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = "SELECT COUNT(*) FROM Users";
        
        SqlCommand command = new SqlCommand(query, connection);
        
        connection.Open();
        return (int)command.ExecuteScalar();
    }
}
```

#### 5. **Transaction Support**

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlTransaction transaction = connection.BeginTransaction();
    
    try
    {
        // Execute multiple commands
        SqlCommand command1 = new SqlCommand(query1, connection, transaction);
        command1.ExecuteNonQuery();
        
        SqlCommand command2 = new SqlCommand(query2, connection, transaction);
        command2.ExecuteNonQuery();
        
        transaction.Commit();
        return true;
    }
    catch
    {
        transaction.Rollback();
        return false;
    }
}
```

---

## 🔒 Security Features

### 1. **Forms Authentication**
- Cookie-based authentication
- Sliding expiration (30 minutes)
- Encrypted authentication tickets
- Role data stored in ticket UserData

### 2. **Password Security**
- SHA-512 hashing with salt
- Strong password requirements:
  - Minimum 8 characters
  - At least one uppercase letter
  - At least one lowercase letter
  - At least one digit
  - At least one special character

### 3. **Account Lockout**
- Lock account after 5 failed login attempts
- 30-minute lockout period
- Automatic unlocking after lockout period

### 4. **Role-Based Authorization**
- Admin role: Full access to AdminPanel
- User role: Access to UserDashboard
- Configured via Web.config location elements

---

## 📱 Application Pages

### Public Pages
- **Default.aspx**: Home page with connection-oriented architecture overview
- **Login.aspx**: User authentication with account lockout
- **Register.aspx**: User registration with password validation

### Protected Pages (Requires Authentication)
- **Logout.aspx**: Logout functionality
- **UserDashboard.aspx**: User-specific dashboard (User role)
- **AdminPanel.aspx**: Admin panel with user management (Admin role)

---

## 🎯 Connection-Oriented Advantages

### 1. **Performance**
- Lower memory footprint (streaming data)
- Faster for large datasets
- No data caching overhead

### 2. **Real-Time Data**
- Always retrieves fresh data from database
- No stale data issues
- Ideal for frequently changing data

### 3. **Scalability**
- Connection pooling reuses connections
- Efficient resource utilization
- Better for high-concurrency scenarios

### 4. **Control**
- Fine-grained control over connections
- Explicit transaction management
- Direct SQL execution

---

## 🛠️ Connection-Oriented Best Practices

### 1. **Always Use Using Statements**
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    // Connection automatically disposed
}
```

### 2. **Close SqlDataReader**
```csharp
using (SqlDataReader reader = command.ExecuteReader())
{
    // Read data
} // Reader automatically closed
```

### 3. **Parameterized Queries**
```csharp
command.Parameters.AddWithValue("@Username", username);
// Prevents SQL injection
```

### 4. **Connection Pooling**
```csharp
// Connection strings with same parameters share pool
// Connections are reused automatically
```

### 5. **Handle Exceptions**
```csharp
try
{
    connection.Open();
    // Execute commands
}
catch (SqlException ex)
{
    // Handle database errors
}
finally
{
    if (connection.State == ConnectionState.Open)
        connection.Close();
}
```

---

## 🔄 Connection-Oriented vs Connectionless

### When to Use Connection-Oriented (Connected)

✅ Large datasets that don't fit in memory  
✅ Real-time data requirements  
✅ Streaming data scenarios  
✅ High-performance data retrieval  
✅ Direct database operations  

### When to Use Connectionless (Disconnected)

✅ Small datasets  
✅ Offline operations  
✅ Data binding to controls  
✅ Batch updates  
✅ Working with multiple tables simultaneously  

---

## 📦 Dependencies

- **ASP.NET Web Forms** (.NET Framework 4.7.2+)
- **System.Data.SqlClient** (ADO.NET SQL Server Provider)
- **System.Web.Security** (Forms Authentication)
- **Bootstrap 5.3** (UI Framework)
- **Font Awesome 6.4** (Icons)

---

## 🧪 Testing the Application

### 1. **User Registration**
- Navigate to Register.aspx
- Create a new account with strong password
- Verify user is created in database

### 2. **User Authentication**
- Login with created credentials
- Verify redirection to UserDashboard
- Check session and authentication cookie

### 3. **Admin Features**
- Login as admin (admin/Admin@123)
- Access AdminPanel
- View user statistics
- Enable/Disable users

### 4. **Account Lockout**
- Attempt 5 failed logins
- Verify account is locked
- Wait 30 minutes or manually unlock in database

### 5. **Role-Based Access**
- Try accessing AdminPanel as regular user
- Verify access denied (redirect to login)

---

## 📊 Connection-Oriented Data Flow

```
User Request → ASPX Page → Code-Behind
                              ↓
                        UserDataAccess
                              ↓
                    SqlConnection.Open()
                              ↓
                        SqlCommand
                              ↓
        ExecuteReader / ExecuteNonQuery / ExecuteScalar
                              ↓
                       SqlDataReader
                              ↓
                    Read Data Stream
                              ↓
                    Connection.Close()
                              ↓
                    Return Data/Result
                              ↓
                    Display in UI
```

---

## 🐛 Troubleshooting

### Connection String Issues
- Verify SQL Server is running
- Check server name in connection string
- Ensure database exists

### Login Issues
- Check username/password in database
- Verify user IsActive = 1
- Check for account lockout

### Authorization Issues
- Verify role is assigned to user in UserRoles table
- Check Web.config location elements
- Ensure Global.asax is loading roles correctly

---

## 🎓 Learning Objectives

After completing this project, you will understand:

1. ✅ Connection-oriented ADO.NET architecture
2. ✅ Manual connection management with SqlConnection
3. ✅ SqlCommand methods (ExecuteReader, ExecuteNonQuery, ExecuteScalar)
4. ✅ SqlDataReader for data streaming
5. ✅ Transaction management in connected mode
6. ✅ Forms Authentication with ASP.NET Web Forms
7. ✅ Role-based authorization
8. ✅ Secure password hashing
9. ✅ Account lockout mechanisms
10. ✅ Connection-oriented vs connectionless comparison

---

## 📚 Additional Resources

- [ADO.NET Overview](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [SqlConnection Class](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection)
- [SqlDataReader Class](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader)
- [Forms Authentication](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/older-versions-security/introduction/)

---

## 📝 Notes

- This project demonstrates **connection-oriented (connected)** approach
- Compare with **Auth_WebForms** project (connectionless approach)
- Connection-oriented is faster for large datasets
- Connectionless is better for offline scenarios
- Both approaches have their use cases in real-world applications

---

## 🚀 Project Completion

This project is complete and demonstrates:
- ✅ Connection-oriented ADO.NET architecture
- ✅ Manual connection lifecycle management
- ✅ SqlDataReader for efficient data retrieval
- ✅ Forms Authentication & Authorization
- ✅ Secure password hashing
- ✅ Role-based access control
- ✅ Account lockout protection

**Happy Coding! 🎉**
