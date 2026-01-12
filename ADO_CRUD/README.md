# Web Forms App with Pure ADO.NET

This is my ASP.NET Web Forms application where I'm learning **pure ADO.NET** - using SqlConnection, SqlCommand, and SqlDataReader directly in the code-behind files. No fancy frameworks or helpers, just the basics.

## Architecture

I kept this super simple - just direct ADO.NET calls from the ASPX pages. No layers or anything:

```
┌─────────────────────────────────────┐
│      ASPX Pages                     │
│  (User Interface)                   │
│  - StudentList.aspx                 │
│  - StudentAdd.aspx                  │
│  - StudentEdit.aspx                 │
│  - CourseList.aspx                  │
└─────────────┬───────────────────────┘
              │ code-behind uses
              ↓
┌─────────────────────────────────────┐
│      ADO.NET Classes                │
│  (Direct Database Access)           │
│  - SqlConnection                    │
│  - SqlCommand                       │
│  - SqlDataReader                    │
│  - SqlDataAdapter                   │
│  - SqlParameter                     │
└─────────────┬───────────────────────┘
              │ executes SQL
              ↓
┌─────────────────────────────────────┐
│      SQL Server                     │
│  (StudentDB Database)               │
│  - Students Table                   │
│  - Courses Table                    │
└─────────────────────────────────────┘
```

## What Makes This Different

**Really minimal approach:**
- No utility classes or helper methods
- No BLL (Business Logic Layer) or DAL (Data Access Layer)
- Just raw ADO.NET code right in each page's code-behind

**Manual connection handling:**
```csharp
SqlConnection conn = null;
try
{
    conn = new SqlConnection(connectionString);
    conn.Open();
    // Use connection
}
finally
{
    if (conn != null && conn.State == ConnectionState.Open)
        conn.Close();
}
```

### **Direct SQL Execution**
Every page directly creates SqlConnection, SqlCommand, and executes queries

## 📊 Database Schema

### Students Table
```sql
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
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);
```

### Courses Table
```sql
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY IDENTITY(1,1),
    CourseCode NVARCHAR(20) NOT NULL UNIQUE,
    CourseName NVARCHAR(200) NOT NULL,
    Credits INT NOT NULL,
    Department NVARCHAR(100),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- SQL Server LocalDB or SQL Server

### Installation

1. **Setup Database**
```powershell
# Run in SQL Server Management Studio:
Database/SetupDatabase.sql
```

2. **Update Connection String**
```xml
<!-- Web.config -->
<connectionStrings>
    <add name="StudentDBConnection" 
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\StudentDB.mdf;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

3. **Build and Run**
- Open `ADO_CRUD.csproj` in Visual Studio
- Build solution (Ctrl + Shift + B)
- Run application (F5)

## 💻 Code Examples

### Example 1: Loading Data with SqlDataReader

```csharp
// Default.aspx.cs - Loading statistics
private void LoadStatistics()
{
    SqlConnection conn = null;
    SqlCommand cmd = null;
    SqlDataReader reader = null;

    try
    {
        // 1. Create connection
        conn = new SqlConnection(connectionString);

        // 2. Create command
        cmd = new SqlCommand("SELECT * FROM vw_Statistics", conn);
        cmd.CommandType = CommandType.Text;

        // 3. Open connection
        conn.Open();

        // 4. Execute reader
        reader = cmd.ExecuteReader();

        // 5. Read data
        if (reader.Read())
        {
            lblTotalStudents.Text = reader["TotalStudents"].ToString();
            lblTotalCourses.Text = reader["TotalCourses"].ToString();
            lblAverageGPA.Text = Convert.ToDecimal(reader["AverageGPA"]).ToString("F2");
            lblExcellent.Text = reader["ExcellentStudents"].ToString();
        }
    }
    catch (Exception ex)
    {
        // Handle error
        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
    }
    finally
    {
        // 6. Clean up - IMPORTANT!
        if (reader != null && !reader.IsClosed)
            reader.Close();

        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 2: Loading GridView with SqlDataAdapter

```csharp
// StudentList.aspx.cs - Loading students
private void LoadStudents()
{
    SqlConnection conn = null;
    SqlDataAdapter adapter = null;
    DataTable dt = new DataTable();

    try
    {
        conn = new SqlConnection(connectionString);
        
        string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                        s.Email, s.GPA, s.IsActive, c.CourseName
                        FROM Students s
                        INNER JOIN Courses c ON s.CourseId = c.CourseId
                        ORDER BY s.StudentId DESC";

        SqlCommand cmd = new SqlCommand(query, conn);
        adapter = new SqlDataAdapter(cmd);
        
        // SqlDataAdapter automatically opens and closes connection
        adapter.Fill(dt);

        gvStudents.DataSource = dt;
        gvStudents.DataBind();
    }
    catch (Exception ex)
    {
        ShowMessage("Error: " + ex.Message, "danger");
    }
    finally
    {
        // SqlDataAdapter closes connection, but explicit cleanup is good practice
        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 3: Inserting with SqlCommand.ExecuteScalar

```csharp
// StudentAdd.aspx.cs - Adding new student
protected void btnSave_Click(object sender, EventArgs e)
{
    SqlConnection conn = null;
    SqlCommand cmd = null;

    try
    {
        conn = new SqlConnection(connectionString);
        
        string query = @"INSERT INTO Students 
                        (StudentNumber, FirstName, LastName, Email, DateOfBirth, CourseId, GPA, IsActive, EnrollmentDate, CreatedDate)
                        VALUES 
                        (@StudentNumber, @FirstName, @LastName, @Email, @DateOfBirth, @CourseId, @GPA, @IsActive, GETDATE(), GETDATE());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        cmd = new SqlCommand(query, conn);
        cmd.CommandType = CommandType.Text;

        // Add parameters manually - prevents SQL injection
        cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text));
        cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(ddlCourse.SelectedValue));
        cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

        conn.Open();
        
        // ExecuteScalar returns the new identity value
        int newStudentId = Convert.ToInt32(cmd.ExecuteScalar());

        Response.Redirect($"StudentList.aspx?success=Student added successfully! (ID: {newStudentId})");
    }
    catch (SqlException sqlEx)
    {
        // Handle SQL-specific errors
        if (sqlEx.Number == 2627) // Unique constraint violation
        {
            ShowMessage("Error: Student number already exists!", "danger");
        }
        else
        {
            ShowMessage("Database Error: " + sqlEx.Message, "danger");
        }
    }
    finally
    {
        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 4: Updating with SqlCommand.ExecuteNonQuery

```csharp
// StudentEdit.aspx.cs - Updating student
protected void btnUpdate_Click(object sender, EventArgs e)
{
    SqlConnection conn = null;
    SqlCommand cmd = null;

    try
    {
        conn = new SqlConnection(connectionString);
        
        string query = @"UPDATE Students 
                        SET StudentNumber = @StudentNumber,
                            FirstName = @FirstName,
                            LastName = @LastName,
                            Email = @Email,
                            DateOfBirth = @DateOfBirth,
                            CourseId = @CourseId,
                            GPA = @GPA,
                            IsActive = @IsActive,
                            ModifiedDate = GETDATE()
                        WHERE StudentId = @StudentId";

        cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@StudentId", Convert.ToInt32(txtStudentId.Text));
        cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@DateOfBirth", Convert.ToDateTime(txtDateOfBirth.Text));
        cmd.Parameters.AddWithValue("@CourseId", Convert.ToInt32(ddlCourse.SelectedValue));
        cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

        conn.Open();
        
        // ExecuteNonQuery returns rows affected
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Response.Redirect("StudentList.aspx?success=Student updated successfully!");
        }
    }
    finally
    {
        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 5: Deleting with Parameters

```csharp
// StudentList.aspx.cs - Deleting student
protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "DeleteStudent")
    {
        try
        {
            int studentId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Students WHERE StudentId = @StudentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    ShowMessage("Student deleted successfully!", "success");
                    LoadStudents();
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Error: " + ex.Message, "danger");
        }
    }
}
```

### Example 6: Search with LIKE Operator

```csharp
// StudentList.aspx.cs - Searching students
protected void btnSearch_Click(object sender, EventArgs e)
{
    string keyword = txtSearch.Text.Trim();

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                        s.Email, s.GPA, s.IsActive, c.CourseName
                        FROM Students s
                        INNER JOIN Courses c ON s.CourseId = c.CourseId
                        WHERE s.FirstName LIKE @Keyword 
                           OR s.LastName LIKE @Keyword
                           OR s.Email LIKE @Keyword
                           OR s.StudentNumber LIKE @Keyword
                        ORDER BY s.StudentId DESC";

        SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);

        gvStudents.DataSource = dt;
        gvStudents.DataBind();
    }
}
```

## 📁 Project Structure

```
ADO_CRUD/
├── Database/
│   └── SetupDatabase.sql          # Database creation script
├── Students/
│   ├── StudentList.aspx           # List students
│   ├── StudentList.aspx.cs        # Direct ADO.NET code
│   ├── StudentAdd.aspx            # Add student form
│   ├── StudentAdd.aspx.cs         # INSERT with SqlCommand
│   ├── StudentEdit.aspx           # Edit student form
│   └── StudentEdit.aspx.cs        # UPDATE with SqlCommand
├── Courses/
│   ├── CourseList.aspx            # List courses
│   └── CourseList.aspx.cs         # SqlDataAdapter usage
├── Styles/
│   └── site.css                   # Application styles
├── Default.aspx                   # Home page
├── Default.aspx.cs                # SqlDataReader example
├── Web.config                     # Connection string
└── README.md
```

## 🔑 ADO.NET Classes Reference

| Class | Purpose | Usage |
|-------|---------|-------|
| `SqlConnection` | Represents connection to SQL Server | `new SqlConnection(connectionString)` |
| `SqlCommand` | Executes SQL commands | `new SqlCommand(query, conn)` |
| `SqlDataReader` | Forward-only data reading | `cmd.ExecuteReader()` |
| `SqlDataAdapter` | Fills DataTable/DataSet | `adapter.Fill(dt)` |
| `SqlParameter` | Query parameters | `cmd.Parameters.AddWithValue()` |
| `DataTable` | In-memory table | `new DataTable()` |
| `CommandType` | Text or StoredProcedure | `CommandType.Text` |

## 📖 ADO.NET Methods

### SqlCommand Methods

```csharp
// ExecuteNonQuery - INSERT, UPDATE, DELETE
int rowsAffected = cmd.ExecuteNonQuery();

// ExecuteScalar - Returns single value (COUNT, MAX, ID, etc.)
object result = cmd.ExecuteScalar();
int count = Convert.ToInt32(cmd.ExecuteScalar());

// ExecuteReader - Returns SqlDataReader for SELECT
SqlDataReader reader = cmd.ExecuteReader();
while (reader.Read())
{
    string name = reader["Name"].ToString();
}
```

### SqlDataAdapter Methods

```csharp
// Fill - Populates DataTable
SqlDataAdapter adapter = new SqlDataAdapter(cmd);
DataTable dt = new DataTable();
adapter.Fill(dt);

// Update - Saves DataTable changes back to database
adapter.Update(dt);
```

## ⚖️ Comparison with Other Approaches

| Aspect | Pure ADO.NET | DbCon Pattern | 3-Tier Architecture |
|--------|--------------|---------------|---------------------|
| **Complexity** | Simplest | Simple | Complex |
| **Code Duplication** | High | Medium | Low |
| **Maintainability** | Low | Medium | High |
| **Learning Curve** | Easy | Easy | Steep |
| **Testability** | Difficult | Medium | Easy |
| **Best For** | Learning | Small Apps | Enterprise |
| **Connection Management** | Manual | Centralized | Encapsulated |
| **Code Location** | Code-behind | Utility class | DAL layer |

## ✅ Best Practices Implemented

1. **Parameterized Queries**: All queries use SqlParameter to prevent SQL injection
2. **Connection Disposal**: Always close connections in finally blocks
3. **Using Statements**: Automatic resource cleanup where applicable
4. **Error Handling**: Try-catch blocks with specific error messages
5. **Input Validation**: ASP.NET validators on all forms
6. **Connection String**: Stored in Web.config for easy configuration

## 🛡️ Security Features

1. **SQL Injection Prevention**: All queries use parameters
2. **Input Validation**: Client and server-side validation
3. **Error Messages**: User-friendly without exposing system details
4. **Connection Security**: Integrated security with SQL Server

## 🎯 When to Use Pure ADO.NET

### ✅ Good For:
- **Learning**: Understanding database fundamentals
- **Simple Apps**: Quick prototypes or small utilities
- **Full Control**: Need complete control over SQL execution
- **Performance**: Optimizing specific queries
- **Demonstrations**: Teaching ADO.NET basics
- **Legacy Code**: Maintaining existing ADO.NET applications

### ❌ Not Recommended For:
- **Production Applications**: Too much code duplication
- **Team Projects**: Hard to maintain consistency
- **Complex Logic**: No separation of concerns
- **Large Applications**: Difficult to scale
- **Testing**: Hard to unit test
- **Long-term Maintenance**: Changes require editing multiple files

## 🧪 Sample Data

The database includes:
- **8 Courses**: CS101, CS201, CS301, CS401, IT101, IT201, BUS101, ENG101
- **12 Students**: With GPAs ranging from 3.40 to 3.95
- **Various Departments**: Computer Science, IT, Business, English
- **Enrollment tracking** and GPA monitoring

## 🌟 Learning Outcomes

By studying this project, you'll learn:
1. Core ADO.NET classes (SqlConnection, SqlCommand, SqlDataReader)
2. Manual connection management and disposal
3. SqlCommand methods (ExecuteNonQuery, ExecuteScalar, ExecuteReader)
4. SqlDataAdapter for filling DataTable
5. Parameterized queries for security
6. Error handling in database operations
7. GridView data binding
8. CRUD operations without abstraction
9. Direct SQL execution from code-behind
10. When NOT to use this approach in production

## 📚 Related Projects

- **DbCon_CRUD**: Centralized database utility class approach
- **ThreeTier_CRUD**: 3-Tier architecture with BLL and DAL layers
- **StoredProcedure_CRUD**: Using stored procedures instead of inline SQL

## ⚠️ Important Notes

**This approach is for LEARNING purposes only!**

For production applications, consider:
- Using Entity Framework or Dapper (ORMs)
- Implementing 3-tier architecture
- Creating a centralized DbCon utility class at minimum
- Using stored procedures for complex operations
- Implementing proper logging and error handling
- Adding unit tests

## 👨‍💻 Author

Created as part of Visual Programming practice series demonstrating fundamental ADO.NET database operations in ASP.NET Web Forms.

---

**Educational Purpose**: This project demonstrates the **most basic** way to work with databases in ASP.NET. It intentionally avoids abstraction to show fundamental ADO.NET concepts. Real-world applications should use better architectural patterns!

