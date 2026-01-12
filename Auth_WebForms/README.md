# Authentication & Authorization System
## ASP.NET Web Forms with ADO.NET Connectionless Approach

A complete authentication and authorization system demonstrating **ADO.NET connectionless (disconnected) architecture** with **ASP.NET Web Forms**.

## 🎯 Key Features

### Authentication
- ✅ **Forms Authentication** - Cookie-based authentication
- ✅ **User Login** - Secure credential validation
- ✅ **User Registration** - New user account creation
- ✅ **Password Security** - SHA512 hashing algorithm
- ✅ **Account Lockout** - Protection against brute force attacks
- ✅ **Remember Me** - Persistent authentication option

### Authorization
- ✅ **Role-based Authorization** - Admin, User, Guest roles
- ✅ **Web.config Authorization** - Declarative authorization rules
- ✅ **Page-level Security** - Protected pages by role
- ✅ **Multi-role Support** - Users can have multiple roles

### ADO.NET Connectionless Architecture
- ✅ **DataAdapter** - All database operations use SqlDataAdapter
- ✅ **DataSet & DataTable** - Disconnected data manipulation
- ✅ **Stored Procedures** - All CRUD operations via SPs
- ✅ **SqlCommandBuilder** - Automatic command generation
- ✅ **Batch Updates** - Efficient bulk operations

## 🏗️ Technology Stack

- **Framework**: ASP.NET Web Forms (.NET Framework 4.8)
- **Data Access**: ADO.NET (SqlDataAdapter, DataSet, DataTable)
- **Database**: SQL Server (LocalDB or Express)
- **Authentication**: Forms Authentication
- **Security**: SHA512 Password Hashing
- **UI Framework**: Bootstrap 5.3.0
- **Icons**: Font Awesome 6.4.0

## 📁 Project Structure

```
Auth_WebForms/
├── Database/
│   └── SetupDatabase.sql           # Database setup with stored procedures
├── DataAccess/
│   └── UserDataAccess.cs          # ADO.NET connectionless data access layer
├── Helpers/
│   └── SecurityHelper.cs          # Password hashing and validation
├── Styles/
│   └── site.css                   # Custom CSS styles
├── Login.aspx/cs                  # Login page
├── Register.aspx/cs               # Registration page
├── Logout.aspx/cs                 # Logout handler
├── Default.aspx/cs                # Home page (authenticated users)
├── UserDashboard.aspx/cs          # User dashboard (all authenticated users)
├── AdminPanel.aspx/cs             # Admin panel (Admin role only)
├── Global.asax/cs                 # Application events and role handling
├── Web.config                     # Configuration and authorization rules
└── Auth_WebForms.csproj          # Project file
```

## 🗄️ Database Schema

### Tables

**Users Table**
- `UserId` (PK, Identity)
- `Username` (Unique)
- `Email` (Unique)
- `PasswordHash` (SHA512)
- `FullName`
- `IsActive`
- `CreatedDate`
- `LastLoginDate`
- `FailedLoginAttempts`
- `IsLockedOut`
- `LockoutEndDate`

**Roles Table**
- `RoleId` (PK, Identity)
- `RoleName` (Unique)
- `Description`
- `CreatedDate`

**UserRoles Table** (Many-to-Many)
- `UserRoleId` (PK, Identity)
- `UserId` (FK)
- `RoleId` (FK)
- `AssignedDate`

### Stored Procedures

1. **sp_AuthenticateUser** - User login with lockout handling
2. **sp_RegisterUser** - New user registration
3. **sp_GetAllUsers** - Retrieve all users with roles
4. **sp_UpdateUserStatus** - Activate/deactivate users
5. **sp_GetUserRoles** - Get user's assigned roles

## 🚀 Setup Instructions

### Step 1: Database Setup

1. Open **SQL Server Management Studio** (SSMS)
2. Connect to your SQL Server instance
3. Open the script: `Database/SetupDatabase.sql`
4. Execute the script to create:
   - Database: `AuthWebFormsDB`
   - Tables: Users, Roles, UserRoles
   - Stored Procedures
   - Default roles and sample users

### Step 2: Update Connection String

Edit `Web.config` and update the connection string:

```xml
<connectionStrings>
  <add name="AuthDbConnection" 
       connectionString="Data Source=YOUR_SERVER;Initial Catalog=AuthWebFormsDB;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**LocalDB Example:**
```
Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AuthWebFormsDB;Integrated Security=True
```

**SQL Server Express Example:**
```
Data Source=.\SQLEXPRESS;Initial Catalog=AuthWebFormsDB;Integrated Security=True
```

### Step 3: Build and Run

1. Open the solution in **Visual Studio 2019/2022**
2. Restore NuGet packages (if any)
3. Build the solution: `Ctrl + Shift + B`
4. Run the application: `F5`

The application will open in your default browser.

## 👤 Default Credentials

### Admin Account
- **Username**: `admin`
- **Password**: `Admin@123`
- **Role**: Admin

### Regular User Account
- **Username**: `john.doe`
- **Password**: `User@123`
- **Role**: User

## 📄 Pages & Access Control

### Public Pages (Anonymous Access)
- ✅ **Login.aspx** - User authentication
- ✅ **Register.aspx** - New user registration

### Protected Pages (Authenticated Users)
- 🔒 **Default.aspx** - Home page (all authenticated users)
- 🔒 **UserDashboard.aspx** - User dashboard (Admin + User roles)

### Admin Only Pages
- 🔐 **AdminPanel.aspx** - Admin panel (Admin role only)

## 🔒 Security Features

### 1. Password Security
```csharp
// SHA512 hashing
string passwordHash = SecurityHelper.HashPassword(password);

// Password strength validation
bool isStrong = SecurityHelper.IsPasswordStrong(password);
```

**Requirements:**
- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 digit
- At least 1 special character

### 2. Account Lockout
After **5 failed login attempts**, the account is locked for **30 minutes**.

### 3. Forms Authentication
Configured in `Web.config`:
```xml
<authentication mode="Forms">
  <forms loginUrl="~/Login.aspx" 
         timeout="30" 
         slidingExpiration="true"
         defaultUrl="~/Default.aspx" />
</authentication>
```

### 4. Role-based Authorization
```xml
<!-- Admin Panel - Admin role only -->
<location path="AdminPanel.aspx">
  <system.web>
    <authorization>
      <allow roles="Admin" />
      <deny users="*" />
    </authorization>
  </system.web>
</location>
```

## 🔌 ADO.NET Connectionless Architecture

### What is Connectionless Approach?

The **connectionless (disconnected) architecture** means:
- Data is retrieved using **SqlDataAdapter**
- Stored in **DataSet** or **DataTable** (in-memory)
- Connection is **closed immediately** after data retrieval
- Data manipulation happens **offline** (disconnected from database)
- Changes are sent back to database in **batch updates**

### Key Components

#### 1. SqlDataAdapter
Bridges between database and DataSet:
```csharp
using (SqlDataAdapter adapter = new SqlDataAdapter(command))
{
    DataSet dataSet = new DataSet();
    adapter.Fill(dataSet, "Users"); // Connection auto-managed
}
```

#### 2. DataSet & DataTable
In-memory data cache:
```csharp
DataTable userTable = new DataTable();
adapter.Fill(userTable); // Disconnected data

// Work with data offline
foreach (DataRow row in userTable.Rows)
{
    string username = row["Username"].ToString();
}
```

#### 3. SqlCommandBuilder
Automatic command generation:
```csharp
SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
// Automatically generates INSERT, UPDATE, DELETE commands
adapter.Update(dataSet, "Users");
```

### Examples in This Project

#### Authentication (Connectionless)
```csharp
public DataTable AuthenticateUser(string username, string passwordHash)
{
    DataTable userTable = new DataTable();
    
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        using (SqlCommand command = new SqlCommand("sp_AuthenticateUser", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            
            // Use DataAdapter - connection managed automatically
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(userTable); // Connection opened and closed
                return userTable; // Disconnected data
            }
        }
    }
}
```

#### Get All Users (Connectionless)
```csharp
public DataSet GetAllUsers()
{
    DataSet dataSet = new DataSet();
    
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        using (SqlCommand command = new SqlCommand("sp_GetAllUsers", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dataSet, "Users"); // Connectionless fill
            }
        }
    }
    
    return dataSet; // Disconnected DataSet
}
```

#### Update with DataSet
```csharp
public bool UpdateUserWithDataSet(int userId, string email, string fullName)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string selectQuery = "SELECT UserId, Email, FullName FROM Users WHERE UserId = @UserId";
        
        using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
        {
            adapter.SelectCommand.Parameters.AddWithValue("@UserId", userId);
            
            // Auto-generate UPDATE command
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Users");
            
            // Modify data offline (disconnected)
            DataRow row = dataSet.Tables["Users"].Rows[0];
            row["Email"] = email;
            row["FullName"] = fullName;
            
            // Send changes back (connectionless update)
            adapter.Update(dataSet, "Users");
            
            return true;
        }
    }
}
```

## 🎨 User Interface

### Login Page
- Modern gradient design
- Client-side validation
- Server-side validation
- Remember me option
- Link to registration

### Registration Page
- Multi-field form with validation
- Password strength indicator
- Role selection
- Email format validation
- Terms acceptance checkbox

### Admin Panel
- User statistics dashboard
- User management GridView
- Activate/Deactivate users
- View user roles
- Real-time data updates

### User Dashboard
- Personalized welcome
- Profile information
- Activity summary
- Access capabilities
- Technical information

## 🔄 Authentication Flow

1. **User enters credentials** on Login.aspx
2. **Password is hashed** using SHA512
3. **DataAdapter calls** `sp_AuthenticateUser` stored procedure
4. **User data retrieved** into DataTable (connectionless)
5. **FormsAuthenticationTicket created** with roles
6. **Encrypted cookie** stored in browser
7. **Redirect** to appropriate page based on role

## 🛡️ Authorization Flow

1. **User requests protected page**
2. **Global.asax.Application_AuthenticateRequest** fires
3. **Authentication cookie decrypted**
4. **Roles extracted** from ticket UserData
5. **GenericPrincipal created** with roles
6. **User.IsInRole()** checked in page
7. **Access granted or denied** based on role

## 📊 Features Demonstrated

### ADO.NET Connectionless Techniques
✅ SqlDataAdapter with Fill() method
✅ DataSet as disconnected data cache
✅ DataTable manipulation offline
✅ SqlCommandBuilder for automatic updates
✅ Batch updates with UpdateBatchSize
✅ Stored procedures with DataAdapter
✅ Parameters with DataAdapter

### Authentication & Authorization
✅ Forms Authentication ticket
✅ Cookie encryption
✅ Role-based authorization
✅ Web.config location elements
✅ Global.asax authentication handling
✅ User.IsInRole() checks
✅ Session management

### Security Best Practices
✅ SHA512 password hashing
✅ Password strength validation
✅ Account lockout mechanism
✅ SQL injection prevention (parameterized queries)
✅ Input validation (client & server)
✅ Secure cookie handling

## 🧪 Testing the Application

### Test Authentication

1. **Login with Admin**
   - Username: `admin`
   - Password: `Admin@123`
   - Should redirect to Admin Panel

2. **Login with User**
   - Username: `john.doe`
   - Password: `User@123`
   - Should redirect to User Dashboard

3. **Failed Login**
   - Try 5 wrong passwords
   - Account should be locked for 30 minutes

### Test Authorization

1. **Access Admin Panel as User**
   - Login as `john.doe`
   - Try to access `/AdminPanel.aspx`
   - Should be denied (403 or redirect)

2. **Access User Dashboard as Admin**
   - Login as `admin`
   - Access `/UserDashboard.aspx`
   - Should have access (Admin can access User pages)

### Test Registration

1. **Register New User**
   - Fill all required fields
   - Use strong password
   - Should create account and redirect to login

2. **Duplicate Username**
   - Try to register with existing username
   - Should show error message

## 📝 Code Highlights

### Global.asax - Role Loading
```csharp
protected void Application_AuthenticateRequest(object sender, EventArgs e)
{
    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
    
    if (authCookie != null)
    {
        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        
        if (authTicket != null && !authTicket.Expired)
        {
            // Extract roles from UserData
            string[] roles = authTicket.UserData.Split(',');
            
            // Create principal with roles
            GenericIdentity identity = new GenericIdentity(authTicket.Name, "Forms");
            GenericPrincipal principal = new GenericPrincipal(identity, roles);
            
            // Set for current request
            Context.User = principal;
        }
    }
}
```

### Login - Creating Auth Ticket
```csharp
FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
    1,                      // version
    username,               // username
    DateTime.Now,           // issue time
    DateTime.Now.AddHours(2), // expiration
    isPersistent,           // persistent
    roles,                  // user data (roles)
    FormsAuthentication.FormsCookiePath
);

string encryptedTicket = FormsAuthentication.Encrypt(ticket);
HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
Response.Cookies.Add(authCookie);
```

## 🐛 Troubleshooting

### Connection String Issues
**Problem**: Cannot connect to database
**Solution**: 
- Check SQL Server service is running
- Verify server name in connection string
- Test connection in SSMS first

### Authentication Not Working
**Problem**: User.IsInRole() always returns false
**Solution**:
- Ensure Global.asax is present
- Check Application_AuthenticateRequest is firing
- Verify roles are in ticket UserData

### Page Access Denied
**Problem**: Authorized user can't access page
**Solution**:
- Check Web.config authorization section
- Verify role name matches exactly
- Clear browser cookies and login again

## 🔧 Configuration Options

### Modify Session Timeout
In `Web.config`:
```xml
<sessionState mode="InProc" timeout="60" />
```

### Modify Authentication Timeout
In `Web.config`:
```xml
<forms timeout="60" slidingExpiration="true" />
```

### Change Lockout Duration
In `SetupDatabase.sql`:
```sql
LockoutEndDate = DATEADD(MINUTE, 30, GETDATE()) -- Change 30 to desired minutes
```

## 📚 Learning Objectives

This project demonstrates:

1. ✅ **ADO.NET Connectionless Architecture**
   - SqlDataAdapter usage
   - DataSet & DataTable operations
   - Disconnected data manipulation

2. ✅ **Forms Authentication**
   - Authentication ticket creation
   - Cookie management
   - Login/logout implementation

3. ✅ **Role-based Authorization**
   - Web.config authorization rules
   - Programmatic role checks
   - Multi-role support

4. ✅ **Security Best Practices**
   - Password hashing
   - Account lockout
   - Input validation

5. ✅ **Stored Procedures**
   - CRUD operations
   - Return values
   - Output parameters

## 🎓 Key Concepts

### Connectionless vs Connected Architecture

| Aspect | Connectionless | Connected |
|--------|---------------|-----------|
| **Connection** | Opens/closes automatically | Manually managed |
| **Component** | SqlDataAdapter, DataSet | SqlCommand, DataReader |
| **Data Storage** | In-memory (DataSet) | Stream-based |
| **Use Case** | Batch operations, offline | Real-time, forward-only |
| **Performance** | Better for bulk | Better for large reads |

### Why Connectionless in This Project?

✅ **Automatic connection management** - No manual Open()/Close()
✅ **Batch updates** - Multiple changes in one database call
✅ **Offline data manipulation** - Work with data without database connection
✅ **GridView binding** - Easy data binding to UI controls
✅ **Better for Web Forms** - Suits stateless HTTP model

## 🚀 Future Enhancements

- [ ] Email verification for registration
- [ ] Password reset functionality
- [ ] Two-factor authentication (2FA)
- [ ] OAuth integration (Google, Facebook)
- [ ] Activity logging and audit trail
- [ ] User profile editing
- [ ] Role assignment by Admin
- [ ] Export users to Excel/PDF
- [ ] Real-time notifications
- [ ] Session management dashboard

## 📖 Additional Resources

- [ASP.NET Forms Authentication](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/older-versions-security/introduction/forms-authentication-configuration-and-advanced-topics-cs)
- [ADO.NET DataSet](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/dataset-datatable-dataview/)
- [SqlDataAdapter](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldataadapter)
- [Role-based Authorization](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/older-versions-security/roles/role-based-authorization-cs)

## 📄 License

This project is created for educational purposes to demonstrate authentication and authorization concepts using ADO.NET connectionless approach.

---

## 📞 Support

For issues or questions:
1. Check the troubleshooting section
2. Review the database setup script
3. Verify connection string configuration
4. Ensure SQL Server is running

---

**Created with ❤️ to demonstrate ADO.NET Connectionless Architecture with ASP.NET Web Forms Authentication & Authorization**

---

## Summary

This project showcases:
- ✅ **ADO.NET Connectionless** (DataAdapter, DataSet, DataTable)
- ✅ **Forms Authentication** with encrypted cookies
- ✅ **Role-based Authorization** (Admin, User, Guest)
- ✅ **SHA512 Password Hashing**
- ✅ **Account Lockout Protection**
- ✅ **Stored Procedures** for all operations
- ✅ **Modern Bootstrap 5 UI**
- ✅ **Complete user management** system
