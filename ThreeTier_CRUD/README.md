# 3-Tier Architecture CRUD Application

This project demonstrates **3-Tier (N-Tier) Architecture** with complete CRUD operations using **ASP.NET Web Forms**. It showcases proper **separation of concerns** with distinct Presentation, Business Logic, and Data Access layers.

---

## 📋 Table of Contents
- [What is 3-Tier Architecture?](#what-is-3-tier-architecture)
- [Architecture Overview](#architecture-overview)
- [Project Structure](#project-structure)
- [Layer Responsibilities](#layer-responsibilities)
- [Why 3-Tier Architecture?](#why-3-tier-architecture)
- [Database Setup](#database-setup)
- [Implementation Details](#implementation-details)
- [Running the Application](#running-the-application)
- [Code Examples](#code-examples)
- [Best Practices](#best-practices)

---

## 🎯 What is 3-Tier Architecture?

**3-Tier Architecture** is a software design pattern that separates an application into three logical layers:

```
┌─────────────────────────────────────┐
│   PRESENTATION LAYER (UI)           │  ← User Interface (ASP.NET Web Forms)
│   - Web Forms (.aspx)               │
│   - User interaction                │
│   - Display logic only              │
└─────────────────────────────────────┘
              ↓ calls ↓
┌─────────────────────────────────────┐
│   BUSINESS LOGIC LAYER (BLL)        │  ← Business Rules & Validation
│   - Validation rules                │
│   - Business constraints            │
│   - Workflow coordination           │
└─────────────────────────────────────┘
              ↓ calls ↓
┌─────────────────────────────────────┐
│   DATA ACCESS LAYER (DAL)           │  ← Database Operations
│   - SQL queries                     │
│   - CRUD operations                 │
│   - No business logic               │
└─────────────────────────────────────┘
              ↓ accesses ↓
┌─────────────────────────────────────┐
│   DATABASE (SQL Server)             │
│   - Tables: Employees, Departments  │
└─────────────────────────────────────┘
```

### **Key Principle: NEVER Skip Layers!**

✅ **Correct Flow:**
```
Presentation → BLL → DAL → Database
```

❌ **WRONG - Don't do this:**
```
Presentation → DAL → Database  (Bypassing BLL)
```

---

## 🏗️ Architecture Overview

### Layer Communication Rules

| Layer | Can Call | Cannot Call | Why? |
|-------|----------|-------------|------|
| **Presentation** | BLL only | DAL, Database | Prevents bypassing business rules |
| **BLL** | DAL, Models | Presentation | Keeps business logic reusable |
| **DAL** | Database | BLL, Presentation | Pure data operations only |

---

## 📁 Project Structure

```
ThreeTier_CRUD/
│
├── Models/                           # Entity Classes (Data Transfer Objects)
│   ├── Employee.cs                   # Employee entity
│   └── Department.cs                 # Department entity
│
├── DAL/ (Data Access Layer)          # Database Operations ONLY
│   ├── DBHelper.cs                   # Database connection & common operations
│   ├── EmployeeDAL.cs                # Employee CRUD operations
│   └── DepartmentDAL.cs              # Department CRUD operations
│
├── BLL/ (Business Logic Layer)       # Validation & Business Rules
│   ├── EmployeeBLL.cs                # Employee business logic
│   ├── DepartmentBLL.cs              # Department business logic
│   └── ValidationException.cs        # Custom validation exception
│
├── Presentation/ (UI Layer)          # Web Forms Pages
│   ├── Default.aspx                  # Home page
│   ├── Employees/
│   │   ├── EmployeeList.aspx         # List employees
│   │   ├── EmployeeAdd.aspx          # Add employee
│   │   ├── EmployeeEdit.aspx         # Edit employee
│   │   └── EmployeeDetails.aspx      # View employee
│   └── Departments/
│       ├── DepartmentList.aspx       # List departments
│       └── DepartmentAdd.aspx        # Add department
│
├── Database/
│   └── SetupDatabase.sql             # Database schema & sample data
│
├── Styles/
│   └── site.css                      # Application styling
│
└── Web.config                        # Connection string & configuration
```

---

## 🎓 Layer Responsibilities

### 1. **Models Layer** (Data Transfer Objects)

**Purpose:** Plain C# classes representing database entities

```csharp
public class Employee
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    // ... more properties
    
    // Computed properties
    public string FullName => $"{FirstName} {LastName}";
}
```

**Responsibilities:**
- Define data structure
- Computed/calculated properties
- No business logic
- No database code

---

### 2. **Data Access Layer (DAL)**

**Purpose:** **PURE DATABASE OPERATIONS** - No business logic!

```csharp
public class EmployeeDAL
{
    // ONLY database operations - NO validation, NO business rules
    
    public List<Employee> GetAllEmployees()
    {
        string query = "SELECT * FROM Employees...";
        // Execute SQL and return data
    }
    
    public int InsertEmployee(Employee employee)
    {
        string query = "INSERT INTO Employees...";
        // Execute INSERT and return new ID
    }
    
    public bool EmailExists(string email)
    {
        string query = "SELECT COUNT(*) FROM Employees WHERE Email = @Email";
        // Check if email exists
    }
}
```

**DAL Should:**
- ✅ Execute SQL queries
- ✅ Map database records to objects
- ✅ Handle database connections
- ✅ Return data or success/failure

**DAL Should NOT:**
- ❌ Validate data (e.g., check email format)
- ❌ Enforce business rules (e.g., "salary must be > $1000")
- ❌ Make business decisions
- ❌ Call other layers

---

### 3. **Business Logic Layer (BLL)**

**Purpose:** **VALIDATION, BUSINESS RULES, WORKFLOW**

```csharp
public class EmployeeBLL
{
    private EmployeeDAL employeeDAL = new EmployeeDAL();
    private DepartmentDAL departmentDAL = new DepartmentDAL();
    
    public int AddEmployee(Employee employee)
    {
        // 1. VALIDATE DATA
        ValidateEmployee(employee);
        
        // 2. BUSINESS RULES
        if (employeeDAL.EmailExists(employee.Email))
        {
            throw new ValidationException("Email already exists");
        }
        
        Department dept = departmentDAL.GetDepartmentById(employee.DepartmentId);
        if (!dept.IsActive)
        {
            throw new ValidationException("Cannot assign to inactive department");
        }
        
        // 3. CALL DAL
        return employeeDAL.InsertEmployee(employee);
    }
    
    private void ValidateEmployee(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new ValidationException("First name is required");
            
        if (employee.FirstName.Length < 2)
            throw new ValidationException("First name must be at least 2 characters");
            
        if (!Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            throw new ValidationException("Invalid email format");
            
        if (employee.Salary < 1000)
            throw new ValidationException("Salary must be at least $1,000");
            
        if (employee.HireDate > DateTime.Now)
            throw new ValidationException("Hire date cannot be in the future");
            
        // ... more validation rules
    }
}
```

**BLL Should:**
- ✅ Validate all input data
- ✅ Enforce business rules
- ✅ Coordinate multiple DAL operations
- ✅ Handle business exceptions
- ✅ Return meaningful error messages

**BLL Should NOT:**
- ❌ Execute SQL queries directly
- ❌ Know about database structure
- ❌ Handle HTTP requests/responses
- ❌ Access ViewState or UI controls

---

### 4. **Presentation Layer (UI)**

**Purpose:** User interface - **ONLY calls BLL, NEVER DAL**

```csharp
public partial class EmployeeAdd : System.Web.UI.Page
{
    // ONLY call BLL - NEVER call DAL directly!
    private EmployeeBLL employeeBLL = new EmployeeBLL();
    
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Employee employee = new Employee
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                // ... get values from controls
            };
            
            // Call BLL - BLL handles ALL validation and business rules
            int newId = employeeBLL.AddEmployee(employee);
            
            Response.Redirect($"EmployeeList.aspx?success=Employee added! (ID: {newId})");
        }
        catch (ValidationException vex)
        {
            // Show business validation error
            ShowMessage(vex.Message, "warning");
        }
        catch (Exception ex)
        {
            // Show general error
            ShowMessage("Error: " + ex.Message, "danger");
        }
    }
}
```

**Presentation Should:**
- ✅ Handle user input/output
- ✅ Call BLL methods
- ✅ Display results
- ✅ Handle UI-specific logic (show/hide controls)

**Presentation Should NOT:**
- ❌ Validate business rules (let BLL do it)
- ❌ Call DAL directly
- ❌ Execute SQL queries
- ❌ Contain business logic

---

## 💡 Why 3-Tier Architecture?

### Benefits

| Benefit | Description | Example |
|---------|-------------|---------|
| **Separation of Concerns** | Each layer has one responsibility | UI changes don't affect database code |
| **Maintainability** | Easy to update without breaking other parts | Change validation rules in BLL only |
| **Testability** | Test each layer independently | Unit test BLL without UI |
| **Reusability** | Share BLL/DAL across different UIs | Use same BLL for Web, Mobile, Desktop |
| **Scalability** | Deploy layers on different servers | Database server, app server, web server |
| **Security** | UI cannot bypass business rules | All operations go through BLL validation |
| **Team Collaboration** | Different teams work on different layers | DB team, business team, UI team |

### Comparison with Other Architectures

**1-Tier (Everything in UI):**
```csharp
protected void btnSave_Click(object sender, EventArgs e)
{
    // ❌ EVERYTHING in one place - BAD!
    
    if (txtFirstName.Text == "") ShowError("Name required");
    if (txtEmail.Text.Length < 5) ShowError("Invalid email");
    
    string sql = "INSERT INTO Employees VALUES (@name, @email)";
    SqlConnection conn = new SqlConnection("...");
    SqlCommand cmd = new SqlCommand(sql, conn);
    cmd.Parameters.AddWithValue("@name", txtFirstName.Text);
    conn.Open();
    cmd.ExecuteNonQuery();
    conn.Close();
}
// Problems: No reusability, hard to test, tightly coupled
```

**2-Tier (UI + DAL only):**
```csharp
// Presentation calls DAL directly - MISSING BLL!
protected void btnSave_Click(object sender, EventArgs e)
{
    // ❌ Basic validation in UI
    if (txtFirstName.Text == "") return;
    
    // ❌ Calls DAL directly - bypasses business rules!
    EmployeeDAL dal = new EmployeeDAL();
    dal.InsertEmployee(employee);
}
// Problems: Business logic scattered, can bypass validation
```

**3-Tier (Correct):**
```csharp
// ✅ Presentation → BLL → DAL
protected void btnSave_Click(object sender, EventArgs e)
{
    EmployeeBLL bll = new EmployeeBLL();
    // BLL handles ALL validation and business rules
    int newId = bll.AddEmployee(employee);
}
// Benefits: All business logic centralized, cannot bypass rules
```

---

## 🗄️ Database Setup

### 1. Create Database

Run [Database/SetupDatabase.sql](Database/SetupDatabase.sql) in SQL Server Management Studio:

```sql
CREATE DATABASE ThreeTierDB;
GO

USE ThreeTierDB;
GO

-- Departments table
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    DepartmentCode NVARCHAR(10) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);

-- Employees table with foreign key
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
```

### 2. Connection String

Update [Web.config](Web.config):

```xml
<connectionStrings>
    <add name="ThreeTierDBConnection" 
         connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ThreeTierDB;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

---

## 🔧 Implementation Details

### Example: Add Employee Flow

**Step 1: User fills form** (Presentation Layer)
```
User enters: John Doe, john@company.com, $5000, etc.
Click "Save Employee" button
```

**Step 2: Presentation calls BLL**
```csharp
// EmployeeAdd.aspx.cs
protected void btnSave_Click(object sender, EventArgs e)
{
    Employee employee = new Employee
    {
        FirstName = txtFirstName.Text.Trim(),
        Email = txtEmail.Text.Trim(),
        Salary = Convert.ToDecimal(txtSalary.Text),
        // ... more properties
    };
    
    // Call BLL
    int newId = employeeBLL.AddEmployee(employee);
}
```

**Step 3: BLL validates and enforces business rules**
```csharp
// EmployeeBLL.cs
public int AddEmployee(Employee employee)
{
    // VALIDATION
    if (string.IsNullOrWhiteSpace(employee.FirstName))
        throw new ValidationException("First name is required");
        
    if (employee.FirstName.Length < 2)
        throw new ValidationException("First name must be at least 2 characters");
        
    if (!Regex.IsMatch(employee.Email, emailPattern))
        throw new ValidationException("Invalid email format");
        
    if (employee.Salary < 1000)
        throw new ValidationException("Salary must be at least $1,000");
        
    // BUSINESS RULES
    if (employeeDAL.EmailExists(employee.Email))
        throw new ValidationException("Email already exists");
        
    Department dept = departmentDAL.GetDepartmentById(employee.DepartmentId);
    if (!dept.IsActive)
        throw new ValidationException("Cannot assign to inactive department");
        
    // If all validation passes, call DAL
    return employeeDAL.InsertEmployee(employee);
}
```

**Step 4: DAL executes database operation**
```csharp
// EmployeeDAL.cs
public int InsertEmployee(Employee employee)
{
    string query = @"INSERT INTO Employees (FirstName, LastName, Email, ...)
                     VALUES (@FirstName, @LastName, @Email, ...);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";
                     
    SqlParameter[] parameters = {
        DBHelper.CreateParameter("@FirstName", employee.FirstName),
        DBHelper.CreateParameter("@LastName", employee.LastName),
        // ... more parameters
    };
    
    return Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
}
```

**Step 5: Return to Presentation**
```
BLL returns new employee ID → Presentation shows success message
```

---

## 🚀 Running the Application

### Prerequisites
- Visual Studio 2019 or later
- SQL Server LocalDB
- .NET Framework 4.7.2 or later

### Setup Steps

1. **Create Database:**
   ```sql
   -- Run Database/SetupDatabase.sql
   ```

2. **Open Project in Visual Studio**

3. **Build Solution:**
   ```
   Ctrl + Shift + B
   ```

4. **Run Application:**
   ```
   F5 or Ctrl + F5
   ```

5. **Navigate to:**
   ```
   http://localhost:PORT/Default.aspx
   ```

---

## 📚 Code Examples

### Example 1: BLL Validation

```csharp
// Business Logic Layer validates ALL input
private void ValidateEmployee(Employee employee)
{
    // NULL checks
    if (employee == null)
        throw new ValidationException("Employee object cannot be null");
        
    // Required fields
    if (string.IsNullOrWhiteSpace(employee.FirstName))
        throw new ValidationException("First name is required");
        
    // Length validation
    if (employee.FirstName.Length < 2)
        throw new ValidationException("First name must be at least 2 characters");
        
    if (employee.FirstName.Length > 50)
        throw new ValidationException("First name cannot exceed 50 characters");
        
    // Format validation
    if (!Regex.IsMatch(employee.FirstName, @"^[a-zA-Z\s'-]+$"))
        throw new ValidationException("First name can only contain letters");
        
    // Email validation
    if (!Regex.IsMatch(employee.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        throw new ValidationException("Invalid email format");
        
    // Range validation
    if (employee.Salary < 1000)
        throw new ValidationException("Salary must be at least $1,000");
        
    if (employee.Salary > 1000000)
        throw new ValidationException("Salary cannot exceed $1,000,000");
        
    // Date validation
    if (employee.HireDate > DateTime.Now.Date)
        throw new ValidationException("Hire date cannot be in the future");
        
    if (employee.HireDate < DateTime.Now.AddYears(-50))
        throw new ValidationException("Hire date cannot be more than 50 years in the past");
}
```

### Example 2: Business Rule Enforcement

```csharp
// BLL enforces business constraints
public int AddEmployee(Employee employee)
{
    ValidateEmployee(employee);
    
    // Business Rule: Email must be unique
    if (employeeDAL.EmailExists(employee.Email))
    {
        throw new ValidationException("Email already exists");
    }
    
    // Business Rule: Department must exist and be active
    Department department = departmentDAL.GetDepartmentById(employee.DepartmentId);
    if (department == null)
    {
        throw new ValidationException("Selected department does not exist");
    }
    if (!department.IsActive)
    {
        throw new ValidationException("Cannot assign employee to an inactive department");
    }
    
    // All rules passed - proceed with insert
    return employeeDAL.InsertEmployee(employee);
}
```

### Example 3: DAL - Pure Database Operations

```csharp
// Data Access Layer - ONLY database operations
public class EmployeeDAL
{
    // No validation here - just data operations
    
    public List<Employee> GetAllEmployees()
    {
        string query = @"SELECT e.*, d.DepartmentName, d.DepartmentCode
                        FROM Employees e
                        INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId";
                        
        List<Employee> employees = new List<Employee>();
        
        using (SqlDataReader reader = DBHelper.ExecuteReader(query))
        {
            while (reader.Read())
            {
                employees.Add(MapReaderToEmployee(reader));
            }
        }
        
        return employees;
    }
    
    public bool EmailExists(string email, int excludeEmployeeId = 0)
    {
        string query = @"SELECT COUNT(*) FROM Employees 
                        WHERE Email = @Email 
                        AND EmployeeId != @ExcludeEmployeeId";
                        
        SqlParameter[] parameters = {
            DBHelper.CreateParameter("@Email", email),
            DBHelper.CreateParameter("@ExcludeEmployeeId", excludeEmployeeId)
        };
        
        int count = Convert.ToInt32(DBHelper.ExecuteScalar(query, parameters));
        return count > 0;
    }
}
```

---

## ✅ Best Practices

### 1. **Layer Separation**

✅ **DO:**
```csharp
// Presentation
EmployeeBLL bll = new EmployeeBLL();
List<Employee> employees = bll.GetAllEmployees();
```

❌ **DON'T:**
```csharp
// Presentation calling DAL directly
EmployeeDAL dal = new EmployeeDAL();  // ❌ WRONG!
List<Employee> employees = dal.GetAllEmployees();
```

### 2. **Validation Location**

✅ **DO:** All validation in BLL
```csharp
// BLL
if (employee.Salary < 1000)
    throw new ValidationException("Salary must be at least $1,000");
```

❌ **DON'T:** Business validation in UI
```csharp
// Presentation
if (Convert.ToDecimal(txtSalary.Text) < 1000)  // ❌ WRONG!
    ShowError("Salary must be at least $1,000");
```

### 3. **Exception Handling**

✅ **DO:** Use custom exceptions from BLL
```csharp
// BLL throws ValidationException
throw new ValidationException("Email already exists");

// Presentation catches and displays
catch (ValidationException vex)
{
    ShowMessage(vex.Message, "warning");
}
```

### 4. **Data Transfer**

✅ **DO:** Use Models/Entities
```csharp
Employee employee = new Employee { FirstName = "John", LastName = "Doe" };
int newId = employeeBLL.AddEmployee(employee);
```

❌ **DON'T:** Pass individual parameters
```csharp
int newId = employeeBLL.AddEmployee("John", "Doe", "john@email.com", ...);  // ❌
```

---

## 📊 Architecture Diagram

```
┌───────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                         │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐       │
│  │EmployeeList  │  │EmployeeAdd   │  │EmployeeEdit  │       │
│  │  .aspx       │  │  .aspx       │  │  .aspx       │       │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘       │
│         └──────────────────┼──────────────────┘               │
│                            │ Calls BLL Only                   │
└────────────────────────────┼──────────────────────────────────┘
                             ↓
┌────────────────────────────┼──────────────────────────────────┐
│              BUSINESS LOGIC LAYER (BLL)                       │
│         ┌──────────────────┴─────────────────┐                │
│         │  EmployeeBLL                       │                │
│         │  - ValidateEmployee()              │                │
│         │  - AddEmployee()                   │                │
│         │  - UpdateEmployee()                │                │
│         │  - DeleteEmployee()                │                │
│         │  - Business rules enforcement      │                │
│         └──────────────────┬─────────────────┘                │
│                            │ Calls DAL                        │
└────────────────────────────┼──────────────────────────────────┘
                             ↓
┌────────────────────────────┼──────────────────────────────────┐
│              DATA ACCESS LAYER (DAL)                          │
│         ┌──────────────────┴─────────────────┐                │
│         │  EmployeeDAL                       │                │
│         │  - GetAllEmployees()               │                │
│         │  - InsertEmployee()                │                │
│         │  - UpdateEmployee()                │                │
│         │  - DeleteEmployee()                │                │
│         │  - Pure SQL operations             │                │
│         └──────────────────┬─────────────────┘                │
│                            │ Accesses Database                │
└────────────────────────────┼──────────────────────────────────┘
                             ↓
                    ┌────────────────┐
                    │   SQL SERVER   │
                    │   ThreeTierDB  │
                    └────────────────┘
```

---

## 🎓 Learning Outcomes

After completing this project, you will understand:

1. **3-Tier Architecture Principles**
   - Separation of concerns
   - Layer responsibilities
   - Communication rules

2. **Business Logic Layer**
   - Validation rules
   - Business constraints
   - Workflow coordination

3. **Data Access Layer**
   - Pure database operations
   - ADO.NET implementation
   - Connection management

4. **Best Practices**
   - Never skip layers
   - Centralized validation
   - Exception handling
   - Code reusability

---

## 📝 Summary

| Layer | Responsibility | Key Files |
|-------|---------------|-----------|
| **Presentation** | User interface, display logic | *.aspx, *.aspx.cs |
| **BLL** | Validation, business rules | EmployeeBLL.cs, DepartmentBLL.cs |
| **DAL** | Database operations | EmployeeDAL.cs, DepartmentDAL.cs |
| **Models** | Data entities | Employee.cs, Department.cs |

**Remember:** Presentation → BLL → DAL → Database (Never skip layers!)

---

**Happy Coding! 🚀**
