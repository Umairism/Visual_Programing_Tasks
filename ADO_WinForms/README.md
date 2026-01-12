# Windows Forms App with Pure ADO.NET

This is my **Windows Forms** desktop application where I'm learning how to work with databases using pure ADO.NET - no Entity Framework or fancy ORMs, just the basics with SqlConnection and SqlCommand.

## How It's Built

I kept the architecture pretty simple - the forms directly call ADO.NET classes. No separate layers or anything complicated. Here's the basic flow:

```
┌─────────────────────────────────────┐
│      Windows Forms UI               │
│  (Desktop Application)              │
│  - MainForm (Dashboard)             │
│  - StudentListForm                  │
│  - StudentAddForm                   │
│  - StudentEditForm                  │
│  - CourseListForm                   │
└─────────────┬───────────────────────┘
              │ code uses
              ↓
┌─────────────────────────────────────┐
│      ADO.NET Classes                │
│  (Direct Database Access)           │
│  - SqlConnection                    │
│  - SqlCommand                       │
│  - SqlDataReader                    │
│  - SqlDataAdapter                   │
│  - DataGridView binding             │
└─────────────┬───────────────────────┘
              │ executes SQL
              ↓
┌─────────────────────────────────────┐
│      SQL Server                     │
│  (StudentDB_WinForms Database)      │
│  - Students Table (15 students)     │
│  - Courses Table (10 courses)       │
└─────────────────────────────────────┘
```

## Main Features

**Pure ADO.NET Approach:**
- Using SqlConnection and SqlCommand directly in each form
- DataGridView to show data in tables
- Modal dialogs for Add/Edit (they pop up over the main form)
- Custom styling to make it look decent

**Windows Forms Controls I Used:**
- **Form** - The main window container
- **Panel** - For organizing stuff
- **DataGridView**: Display tabular data
- **TextBox**: Data input
- **ComboBox**: Dropdown selection
- **DateTimePicker**: Date selection
- **Button**: Actions
- **CheckBox**: Boolean values

**Database:**
The app uses StudentDB_WinForms database with:
- **Students table** - 15 sample students with phone numbers
- **Courses table** - 10 different courses
- **Views** - vw_StudentSummary and vw_Statistics for reporting

## Setup

**Prerequisites:**
- Visual Studio 2019 or newer
- .NET Framework 4.7.2 or later (I upgraded mine to .NET 10)
- SQL Server LocalDB or SQL Server

**Installation Steps:**

1. **Create the database**
```powershell
# Open Database/SetupDatabase.sql in SQL Server Management Studio
# Just run it - it creates everything you need
```

2. **Update connection string** (if needed)
```xml
<!-- Check App.config file -->
<connectionStrings>
    <add name="StudentDBConnection" 
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;..." />
</connectionStrings>
```

3. **Build and run**
- Open ADO_WinForms.csproj in Visual Studio
- Press Ctrl + Shift + B to build
- Press F5 to run

## Code Examples

### Example 1: MainForm - Loading Statistics with SqlDataReader

This is how I load the dashboard statistics using SqlDataReader:

```csharp
// MainForm.cs
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
            lblExcellentStudents.Text = reader["ExcellentStudents"].ToString();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        // 6. Cleanup
        if (reader != null && !reader.IsClosed)
            reader.Close();

        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 2: StudentListForm - DataGridView with SqlDataAdapter

For the student list, I use SqlDataAdapter because it's easier for binding to DataGridView:

```csharp
// StudentListForm.cs
private void LoadStudents()
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"SELECT s.StudentId, s.StudentNumber, s.FirstName, s.LastName, 
                            s.Email, s.Phone, s.GPA, c.CourseName, 
                            CASE WHEN s.IsActive = 1 THEN 'Active' ELSE 'Inactive' END AS Status
                            FROM Students s
                            INNER JOIN Courses c ON s.CourseId = c.CourseId
                            ORDER BY s.StudentId DESC";

            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            
            // SqlDataAdapter automatically opens and closes connection
            adapter.Fill(dt);

            // Bind to DataGridView
            dgvStudents.DataSource = dt;

            // Style columns
            dgvStudents.Columns["StudentId"].HeaderText = "ID";
            dgvStudents.Columns["StudentId"].Width = 50;
            dgvStudents.Columns["StudentNumber"].HeaderText = "Student #";
            // ... more column styling
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

### Example 3: StudentAddForm - INSERT with ExecuteScalar

When adding a new student, I use ExecuteScalar to get the new ID back:

```csharp
// StudentAddForm.cs
private void BtnSave_Click(object sender, EventArgs e)
{
    SqlConnection conn = null;
    SqlCommand cmd = null;

    try
    {
        conn = new SqlConnection(connectionString);
        
        string query = @"INSERT INTO Students 
                        (StudentNumber, FirstName, LastName, Email, Phone, 
                         DateOfBirth, CourseId, GPA, IsActive, EnrollmentDate, CreatedDate)
                        VALUES 
                        (@StudentNumber, @FirstName, @LastName, @Email, @Phone, 
                         @DateOfBirth, @CourseId, @GPA, @IsActive, GETDATE(), GETDATE());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        cmd = new SqlCommand(query, conn);
        cmd.CommandType = CommandType.Text;

        // Add parameters
        cmd.Parameters.AddWithValue("@StudentNumber", txtStudentNumber.Text.Trim());
        cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
        cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
        cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value.Date);
        cmd.Parameters.AddWithValue("@CourseId", ((ComboBoxItem)cmbCourse.SelectedItem).Value);
        cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

        conn.Open();
        
        // ExecuteScalar returns new ID
        int newId = Convert.ToInt32(cmd.ExecuteScalar());

        MessageBox.Show($"Student added successfully! (ID: {newId})", "Success",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    catch (SqlException sqlEx)
    {
        if (sqlEx.Number == 2627)
        {
            MessageBox.Show("Student number already exists!", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    finally
    {
        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 4: StudentEditForm - UPDATE with ExecuteNonQuery

For updating existing students, I use ExecuteNonQuery since we don't need a return value:

```csharp
// StudentEditForm.cs
private void BtnUpdate_Click(object sender, EventArgs e)
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
                            Phone = @Phone,
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
        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
        cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value.Date);
        cmd.Parameters.AddWithValue("@CourseId", ((ComboBoxItem)cmbCourse.SelectedItem).Value);
        cmd.Parameters.AddWithValue("@GPA", Convert.ToDecimal(txtGPA.Text));
        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

        conn.Open();
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            MessageBox.Show("Student updated successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
    finally
    {
        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}
```

### Example 5: DELETE with Confirmation

Deleting records - pretty straightforward:

```csharp
// StudentListForm.cs
private void DeleteStudent(int studentId)
{
    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Students WHERE StudentId = @StudentId";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Student deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

This is how I populate the course dropdown list:

### Example 6: Loading ComboBox with SqlDataReader

```csharp
// StudentAddForm.cs
private void LoadCourses()
{
    SqlConnection conn = null;
    SqlCommand cmd = null;
    SqlDataReader reader = null;

    try
    {
        conn = new SqlConnection(connectionString);
        cmd = new SqlCommand("SELECT CourseId, CourseName FROM Courses WHERE IsActive = 1 ORDER BY CourseName", conn);

        conn.Open();
        reader = cmd.ExecuteReader();

        cmbCourse.Items.Clear();

        while (reader.Read())
        {
            cmbCourse.Items.Add(new ComboBoxItem
            {
                Text = reader["CourseName"].ToString(),
                Value = Convert.ToInt32(reader["CourseId"])
            });
        }

        cmbCourse.DisplayMember = "Text";
        cmbCourse.ValueMember = "Value";

        if (cmbCourse.Items.Count > 0)
            cmbCourse.SelectedIndex = 0;
    }
    finally
    {
        if (reader != null && !reader.IsClosed)
            reader.Close();

        if (conn != null && conn.State == ConnectionState.Open)
            conn.Close();
    }
}

// Helper class for ComboBox items
private class ComboBoxItem
{
    public string Text { get; set; }
    public int Value { get; set; }
}
```Project Structure

Here's how the files are organized:

```
ADO_WinForms/
├── Database/
│   └── SetupDatabase.sql          # Run this to create the database
├── Forms/
│   ├── MainForm.cs                # Main dashboard
│   ├── StudentListForm.cs         # Shows all students
│   ├── StudentAddForm.cs          # Add new student
│   ├── StudentEditForm.cs         # Edit existing student
│   └── CourseListForm.cs          # Shows courses
├── Program.cs                     # Where the app starts
├── App.config                     # Connection string goes hert
├── App.config                     # Connection string
├── ADO_WinForms.csproj           # Project file
└── README.md
```

## 🎨 Windows Forms Controls Used

| Control | Purpose | Usage |
|---------|---------|-------|
| `Form` | Main window container | `new Form()` |
| `Panel` | Layout grouping | `new Panel()` |
| `Label` | Display text | `new Label()` |
| `TextBox` | Text input | `new TextBox()` |
| `Button` | Actions | `new Button()` |
| `Windows Forms Controls I Learned

| Control | What I Used It For | How to Use |
|---------|-------------------|-----------|
| `Form` | The main window | `new Form()` |
| `Panel` | Grouping things together | `new Panel()` |
| `Label` | Showing text | `new Label()` |
| `TextBox` | Getting user input | `new TextBox()` |
| `Button` | Clickable actions | `new Button()` |
| `DataGridView` | Showing data in a table | `new DataGridView()` |
| `ComboBox` | Dropdown list | `new ComboBox()` |
| `DateTimePicker` | Picking dates | `new DateTimePicker()` |
| `CheckBox` | Yes/no options | `new CheckBox()` |
| `MessageBox` | Pop-up message| Direct event handling |
| **State** | ViewState | Form fields |
| *Web Forms vs Windows Forms - What's Different?

| Thing | Web Forms | Windows Forms |
|-------|-----------|---------------|
| *Good Practices I Tried to Follow

1. **Use parameters** - Prevents SQL injection (where hackers try to mess with your queries)
2. **Always close connections** - Otherwise they leak and cause problems
3. **Using statements** - Automatically cleans things up
4. **Modal dialogs** - ShowDialog() makes the Add/Edit forms pop up over the main one
5. **Error handling** - Try-catch blocks with MessageBox to show errors nicely
6. **Validate inputs** - Check that required fields aren't empty
7. **DialogResult** - Return OK or Cancel from the modal forms
8. **Style the DataGridView** - Make columns the right size and look decent

1. Security Things

- **SQL Injection**: Using SqlParameter instead of string concatenation
- **Input checking**: Making sure required fields have values
- **Nice error messages**: Using MessageBox instead of crashing
- **Connection security**: Using Integrated Security (Windows authentication)
- **Modal forms**: Can't edit two records at once

###When Windows Forms Makes Sense

**Good for:**
- Desktop apps that work with local data
- Internal company tools
- Apps that work offline
- When you need complex UI controls
- Direct database access
- Quick development with the drag-and-drop designer

**Not so good for:**
- Web applications (use ASP.NET instead)
- Mobile apps (need something else like MAUI)
- Modern looking UI (WPF looks better)
- Running on Mac or Linux (Windows Forms is Windows-only)
- CWhat the App Does

- **Dashboard** - Shows stats and has buttons to navigate
- **Student list** - Search, filter, see all students in a grid
- **Add student** - Modal popup with validation
- **Edit student** - Loads the data and lets you update it
- **Delete student** - Asks for confirmation first
- **Course list** - View courses and how many students enrolled
- **Clean UI** - Tried to make it look organized

## Comparison with My Other Projects

| Project | Type | Approach | Complexity | Best For |
|---------|------|----------|------------|----------|
| **ADO_WinForms** | Desktop | Pure ADO.NET | Pretty simple | Desktop apps |
| **ADO_CRUD** | Web | Pure ADO.NET | Simple | Learning the basics |
| **DbCon_CRUD** | Web | Centralized class | Still simple | Small web apps |
| **ThreeTier_CRUD** | Web | 3 layers | More complex | Real enterprise apps |

## What I Learned

Working on this project taught me:
1. How to structure a Windows Forms application
2. Using pure ADO.NET in desktop apps (no ORM)
3. Binding data to DataGridView
4. Modal dialogs for Add/Edit operations
5. Handling form events properly
6. Different Windows Forms controls and when to use them
7. Basic desktop UI design
8. When to use SqlDataAdapter vs SqlDataReader
9. How to bind data to ComboBox controls
10. Using MessageBox for user feedback

TooHow It Looks (roughly)

**MainForm Dashboard:**
Has some colored boxes showing stats, navigation buttons, pretty basic layout.

**StudentListForm:**
Big DataGridView showing all students, with Edit/Delete buttons, search box, and an "Add Student" button at the top.

**StudentAddForm:**
Input fields organized in a panel, dropdown for selecting course, date picker for birth date, Save and Cancel buttons at the bottom.

---

Part of my Visual Programming practice - learning how desktop apps work with ADO.NET and SQL Server

Created as part of Visual Programming practice series demonstrating ADO.NET database operations in Windows Forms desktop applications.

---

**Educational Purpose**: This project demonstrates pure ADO.NET with Windows Forms. For production apps, consider Entity Framework, WPF, or modern MAUI with proper architecture patterns!

