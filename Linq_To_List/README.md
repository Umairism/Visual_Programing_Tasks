# Student Management System - ASP.NET Web Forms with LINQ to List

This is a complete ASP.NET Web Forms application demonstrating CRUD operations using LINQ to List.

## Features

- **Create**: Add new students with validation
- **Read**: View all students in a GridView with search functionality
- **Update**: Edit existing student information
- **Delete**: Remove students with confirmation
- **Search**: Find students by name, email, or course using LINQ
- **LINQ Operations**: All data operations use LINQ to List methods

## LINQ Operations Used

### In StudentRepository.cs:
- `OrderBy()` - Sort students by name
- `FirstOrDefault()` - Find student by ID
- `Where()` - Filter students by search criteria
- `Contains()` - Search within text fields
- `Count()` - Get total number of students
- `ToList()` - Convert query results to List

## Project Structure

```
Taska/
├── Models/
│   └── Student.cs              # Student data model
├── DataAccess/
│   └── StudentRepository.cs    # CRUD operations using LINQ
├── Styles/
│   └── Site.css               # Application styling
├── Default.aspx               # Main listing page
├── Default.aspx.cs            # Code-behind for listing
├── AddEdit.aspx               # Add/Edit form page
├── AddEdit.aspx.cs            # Code-behind for form
├── Web.config                 # Application configuration
├── Global.asax                # Application events
└── README.md                  # This file
```

## How to Run

1. Open the project in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5 or Ctrl+F5)
4. The browser will open to Default.aspx

## Student Fields

- **ID**: Auto-generated unique identifier
- **Name**: Student's full name
- **Email**: Email address with validation
- **Course**: Selected from dropdown list
- **Enrollment Date**: Date picker for enrollment
- **Phone**: Contact number

## CRUD Operations

### Create (Add New Student)
1. Click "Add New Student" button on Default.aspx
2. Fill in all required fields
3. Click "Save" to add the student using LINQ

### Read (View Students)
1. All students are displayed on Default.aspx
2. Use search box to filter students using LINQ Where() and Contains()
3. Click "Show All" to reset the view

### Update (Edit Student)
1. Click "Edit" button next to a student
2. Modify the information
3. Click "Save" to update using LINQ FirstOrDefault()

### Delete (Remove Student)
1. Click "Delete" button next to a student
2. Confirm the deletion
3. Student is removed using LINQ FirstOrDefault() and Remove()

## Technologies Used

- ASP.NET Web Forms
- C# 
- LINQ to List
- HTML5 & CSS3
- JavaScript (for confirmations)

## Key Learning Points

1. **LINQ Query Syntax**: Using method syntax for data operations
2. **Static List**: In-memory data storage using static List<Student>
3. **Web Forms Events**: Button clicks and GridView commands
4. **Validation**: Client and server-side validation
5. **Responsive Design**: Mobile-friendly CSS layout

## Notes

- This application uses an in-memory List<Student> for data storage
- Data will be reset when the application restarts
- Sample data is automatically loaded on first run
- All CRUD operations are performed using LINQ methods

## Author

Created as a demonstration of LINQ to List with ASP.NET Web Forms
