# Student Management - ASP.NET Core MVC with EF Core

This is my ASP.NET Core MVC application using Entity Framework Core with Code First approach. Pretty standard CRUD operations for managing students.

## What It Does

- **CRUD operations** - Create, Read, Update, Delete students (the basics)
- **Entity Framework Core** - Using Code First (define models in code, EF creates the database)
- **SQL Server** - LocalDB for development
- **Validation** - Both client and server-side
- **Search** - Can search students by name, email, or course
- **Responsive design** - Looks decent on phones too, using Bootstrap 5
- **Code First Migrations** - Database schema is managed through code

## Tech I'm Using

- ASP.NET Core 6.0 MVC
- Entity Framework Core 6.0
- SQL Server (LocalDB)
- Bootstrap 5 for styling
- Font Awesome icons
- jQuery Validation

## Project Structure

```
Core_MVC_EF/
├── Controllers/
│   └── StudentsController.cs      # CRUD action methods
├── Data/
│   └── ApplicationDbContext.cs    # EF DbContext with Code First
├── Models/
│   └── Student.cs                 # Student entity model
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml         # Main layout
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Students/
│   │   ├── Index.cshtml           # List all students
│   │   ├── Create.cshtml          # Add new student
│   │   ├── Edit.cshtml            # Edit student
│   │   ├── Details.cshtml         # View student details
│   │   └── Delete.cshtml          # Delete confirmation
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   └── css/
│       └── site.css               # Custom styles
├── appsettings.json               # Configuration
├── Program.cs                     # Application entry point
└── Core_MVC_EF.csproj            # Project file

```

## Database Setup

### Connection String
The application uses SQL Server LocalDB by default:
```
Server=(localdb)\\mssqllocaldb;Database=StudentManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true
```

### Code First Migration Commands

1. **Add Migration** (Create migration files):
```bash
dotnet ef migrations add InitialCreate
```

2. **Update Database** (Apply migrations to database):
```bash
dotnet ef database update
```

3. **Remove Last Migration** (If needed):
```bash
dotnet ef migrations remove
```

4. **View Migration SQL** (See what will be executed):
```bash
dotnet ef migrations script
```

## Student Model Properties

- **Id** (int): Primary Key, auto-generated
- **Name** (string): Required, 2-100 characters
- **Email** (string): Required, unique, valid email format
- **Course** (string): Required
- **EnrollmentDate** (DateTime): Required
- **Phone** (string): Required, valid phone format
- **Address** (string): Optional, max 500 characters
- **IsActive** (bool): Default true
- **CreatedDate** (DateTime): Auto-set to current date

## Entity Framework Features Demonstrated

### Code First Approach
- Model classes define database schema
- Data annotations for validation and constraints
- Fluent API configuration in DbContext
- Automatic database creation and updates

### DbContext Configuration
- Entity configuration with Fluent API
- Unique index on Email field
- Data seeding for initial records
- Relationship mapping

### CRUD Operations
1. **Create**: `_context.Add()` + `SaveChangesAsync()`
2. **Read**: `_context.Students.ToListAsync()`, `FirstOrDefaultAsync()`
3. **Update**: `_context.Update()` + `SaveChangesAsync()`
4. **Delete**: `_context.Remove()` + `SaveChangesAsync()`

### LINQ Queries
- `Where()` for filtering
- `OrderBy()` for sorting
- `Contains()` for search
- `Any()` for existence checks

## How to Run

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or VS Code

### Steps

1. **Restore packages**:
```bash
cd Core_MVC_EF
dotnet restore
```

2. **Create database**:
```bash
dotnet ef database update
```

3. **Run the application**:
```bash
dotnet run
```

4. **Open browser** and navigate to:
```
https://localhost:5001
```

## CRUD Operations Guide

### Create (Add New Student)
1. Click "Add New Student" button
2. Fill in required fields with validation
3. Submit form to save to database
4. Redirects to Index page on success

### Read (View Students)
- **Index**: Lists all students in a table
- **Details**: Shows complete information for one student
- **Search**: Filter students using search box

### Update (Edit Student)
1. Click "Edit" button on any student
2. Modify information in form
3. Submit to update database
4. Returns to Index page

### Delete (Remove Student)
1. Click "Delete" button
2. Confirm deletion on confirmation page
3. Student permanently removed from database

## Validation Rules

- **Name**: Required, 2-100 characters
- **Email**: Required, valid format, unique
- **Course**: Required, dropdown selection
- **Enrollment Date**: Required, valid date
- **Phone**: Required, valid phone format
- **Address**: Optional, max 500 characters

## Sample Data

The application seeds 3 sample students:
1. John Doe - Computer Science
2. Jane Smith - Business Administration
3. Mike Johnson - Engineering

## Database Migrations

### InitialCreate Migration Includes:
- Students table creation
- All columns with appropriate data types
- Primary key on Id
- Unique index on Email
- Seed data for 3 students

## Features Highlights

✅ **Code First Approach**: Database from models
✅ **Async Operations**: All database calls are async
✅ **Data Validation**: Client and server-side
✅ **Error Handling**: Try-catch blocks and ModelState
✅ **Responsive UI**: Works on all devices
✅ **Search Functionality**: Real-time filtering
✅ **TempData Messages**: Success notifications
✅ **Entity Tracking**: EF change tracking
✅ **Migrations**: Database versioning

## Common EF Commands Reference

```bash
# List all migrations
dotnet ef migrations list

# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations to database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Drop database
dotnet ef database drop

# View DbContext info
dotnet ef dbcontext info

# Generate SQL script
dotnet ef migrations script
```

## Troubleshooting

### Issue: "Unable to resolve service for type 'ApplicationDbContext'"
**Solution**: Ensure DbContext is registered in Program.cs

### Issue: Migration command not found
**Solution**: Install EF Core tools:
```bash
dotnet tool install --global dotnet-ef
```

### Issue: Database connection fails
**Solution**: Check SQL Server LocalDB is installed and running

## Author

Created as a demonstration of ASP.NET Core MVC with Entity Framework Core Code First approach.
