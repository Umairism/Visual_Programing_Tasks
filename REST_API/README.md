# REST API for Student Management

My first proper RESTful API built with ASP.NET Core. I'm using Entity Framework Core and the Repository pattern to keep things organized.

## What I Implemented

- **RESTful design** - Using proper HTTP methods (GET, POST, PUT, DELETE) and status codes
- **Repository pattern** - Separates data access from business logic
- **Entity Framework Core** - Code First approach, pretty cool actually
- **DTOs** - Data Transfer Objects so I don't expose my entity models directly
- **AutoMapper** - Automatically maps between DTOs and entities
- **Swagger** - Interactive API documentation (really useful for testing)
- **CORS enabled** - So other apps can call my API
- **Async/await** - Everything's asynchronous
- **Logging** - Built-in logging for debugging
- **Validation** - Data annotations for input validation

## Tech Stack

- **ASP.NET Core 6.0** - The web API framework
- **Entity Framework Core 6.0** - ORM for database stuff
- **SQL Server (LocalDB)** - Development database
- **AutoMapper** - For mapping objects
- **Swagger/OpenAPI** - API docs
- **Repository Pattern** - Design pattern I'm learning

## Project Structure

```
REST_API/
├── Controllers/
│   └── StudentsController.cs      # API endpoints
├── Data/
│   └── ApplicationDbContext.cs    # EF DbContext
├── DTOs/
│   ├── StudentCreateDto.cs        # Create request DTO
│   ├── StudentUpdateDto.cs        # Update request DTO
│   └── StudentReadDto.cs          # Response DTO
├── Models/
│   └── Student.cs                 # Entity model
├── Profiles/
│   └── StudentProfile.cs          # AutoMapper profiles
├── Repository/
│   ├── IRepository.cs             # Generic repository interface
│   ├── IStudentRepository.cs      # Student repository interface
│   └── StudentRepository.cs       # Student repository implementation
├── appsettings.json               # Configuration
├── Program.cs                     # Application startup
└── REST_API.csproj               # Project file
```

## Repository Pattern Implementation

### Generic Repository Interface (`IRepository<T>`)
```csharp
- GetAllAsync()           // Get all entities
- GetByIdAsync(id)        // Get by ID
- AddAsync(entity)        // Create new
- UpdateAsync(entity)     // Update existing
- DeleteAsync(id)         // Delete by ID
- ExistsAsync(id)         // Check existence
- SaveChangesAsync()      // Save to database
```

### Student Repository Interface (`IStudentRepository`)
Extends generic repository with student-specific methods:
```csharp
- SearchStudentsAsync(searchTerm)
- GetStudentsByCourseAsync(course)
- GetActiveStudentsAsync()
- GetStudentByEmailAsync(email)
- EmailExistsAsync(email, excludeId)
- GetTotalCountAsync()
```

## API Endpoints

### Base URL: `https://localhost:5001/api`

| Method | Endpoint | Description |
|--------|----------|-------------|
| **GET** | `/students` | Get all students |
| **GET** | `/students/{id}` | Get student by ID |
| **POST** | `/students` | Create new student |
| **PUT** | `/students/{id}` | Update student |
| **DELETE** | `/students/{id}` | Delete student |
| **GET** | `/students/search?searchTerm=...` | Search students |
| **GET** | `/students/course/{course}` | Get students by course |
| **GET** | `/students/active` | Get active students only |
| **GET** | `/students/count` | Get total student count |

## HTTP Status Codes

- **200 OK** - Successful GET, PUT, DELETE
- **201 Created** - Successful POST
- **400 Bad Request** - Invalid input
- **404 Not Found** - Resource not found
- **409 Conflict** - Email already exists
- **500 Internal Server Error** - Server error

## Request/Response Examples

### 1. GET All Students
```http
GET /api/students
```

**Response (200 OK):**
```json
{
  "success": true,
  "count": 4,
  "data": [
    {
      "Id": 1,
      "Name": "Alice Johnson",
      "Email": "alice.johnson@email.com",
      "Course": "Computer Science",
      "EnrollmentDate": "2025-09-01T00:00:00",
      "Phone": "111-222-3333",
      "Address": "123 Tech Street, Silicon Valley",
      "IsActive": true,
      "CreatedDate": "2026-01-13T10:00:00Z",
      "UpdatedDate": null
    }
  ]
}
```

### 2. GET Student by ID
```http
GET /api/students/1
```

**Response (200 OK):**
```json
{
  "success": true,
  "data": {
    "Id": 1,
    "Name": "Alice Johnson",
    "Email": "alice.johnson@email.com",
    "Course": "Computer Science",
    ...
  }
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Student with ID 99 not found"
}
```

### 3. POST Create Student
```http
POST /api/students
Content-Type: application/json

{
  "Name": "John Smith",
  "Email": "john.smith@email.com",
  "Course": "Data Science",
  "EnrollmentDate": "2026-01-15",
  "Phone": "555-666-7777",
  "Address": "456 Data Street",
  "IsActive": true
}
```

**Response (201 Created):**
```json
{
  "success": true,
  "message": "Student created successfully",
  "data": {
    "Id": 5,
    "Name": "John Smith",
    ...
  }
}
```

**Response (409 Conflict):**
```json
{
  "success": false,
  "message": "A student with this email already exists"
}
```

### 4. PUT Update Student
```http
PUT /api/students/1
Content-Type: application/json

{
  "Name": "Alice Johnson Updated",
  "Email": "alice.updated@email.com",
  "Course": "Computer Science",
  "EnrollmentDate": "2025-09-01",
  "Phone": "111-222-3333",
  "Address": "123 Tech Street, Silicon Valley",
  "IsActive": true
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Student updated successfully",
  "data": { ... }
}
```

### 5. DELETE Student
```http
DELETE /api/students/1
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Student deleted successfully"
}
```

### 6. Search Students
```http
GET /api/students/search?searchTerm=computer
```

**Response (200 OK):**
```json
{
  "success": true,
  "searchTerm": "computer",
  "count": 1,
  "data": [ ... ]
}
```

### 7. Get Students by Course
```http
GET /api/students/course/Computer%20Science
```

### 8. Get Active Students
```http
GET /api/students/active
```

### 9. Get Total Count
```http
GET /api/students/count
```

**Response (200 OK):**
```json
{
  "success": true,
  "totalCount": 4
}
```

## Setup Instructions

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server LocalDB
- Postman or similar API testing tool (optional)

### Installation Steps

1. **Navigate to project directory:**
```bash
cd "e:\Visual Programing Practice\Taska\REST_API"
```

2. **Restore NuGet packages:**
```bash
dotnet restore
```

3. **Create database migration:**
```bash
dotnet ef migrations add InitialCreate
```

4. **Apply migration to database:**
```bash
dotnet ef database update
```

5. **Run the application:**
```bash
dotnet run
```

6. **Access Swagger UI:**
Open browser and navigate to: `https://localhost:5001`

## Testing the API

### Option 1: Swagger UI (Recommended)
1. Run the application
2. Open `https://localhost:5001` in browser
3. Swagger UI provides interactive testing interface
4. Try out all endpoints directly from the browser

### Option 2: Postman
1. Import the API endpoints into Postman
2. Set base URL: `https://localhost:5001/api`
3. Test each endpoint with different payloads

### Option 3: cURL
```bash
# Get all students
curl -X GET https://localhost:5001/api/students

# Create student
curl -X POST https://localhost:5001/api/students \
  -H "Content-Type: application/json" \
  -d '{"Name":"Test","Email":"test@email.com","Course":"Test","EnrollmentDate":"2026-01-15","Phone":"123-456-7890","IsActive":true}'
```

## Database Schema

### Students Table
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PRIMARY KEY, IDENTITY |
| Name | nvarchar(100) | NOT NULL |
| Email | nvarchar(100) | NOT NULL, UNIQUE |
| Course | nvarchar(100) | NOT NULL |
| EnrollmentDate | datetime2 | NOT NULL |
| Phone | nvarchar(20) | NOT NULL |
| Address | nvarchar(500) | NULL |
| IsActive | bit | NOT NULL, DEFAULT 1 |
| CreatedDate | datetime2 | NOT NULL, DEFAULT GETUTCDATE() |
| UpdatedDate | datetime2 | NULL |

### Indexes
- **IX_Student_Email** (Unique) - Email column
- **IX_Student_Course** - Course column

## Repository Pattern Benefits

1. **Separation of Concerns**: Data access logic separated from business logic
2. **Testability**: Easy to mock repository for unit testing
3. **Maintainability**: Changes to data access don't affect controllers
4. **Reusability**: Repository methods can be reused across controllers
5. **Abstraction**: Controllers don't need to know about EF Core
6. **Flexibility**: Easy to swap out data access implementations

## AutoMapper Configuration

DTOs are automatically mapped using AutoMapper profiles:
- `Student` ↔ `StudentReadDto`
- `StudentCreateDto` → `Student`
- `StudentUpdateDto` → `Student`

## Validation Rules

All validation is handled through Data Annotations:
- **Name**: Required, 2-100 characters
- **Email**: Required, valid email format, unique
- **Course**: Required, max 100 characters
- **EnrollmentDate**: Required, valid date
- **Phone**: Required, valid phone format
- **Address**: Optional, max 500 characters
- **IsActive**: Boolean, default true

## Error Handling

The API returns consistent error responses:
```json
{
  "success": false,
  "message": "Error description",
  "errors": { ... } // ModelState errors if applicable
}
```

## Logging

Logging is configured for:
- API endpoint calls
- Database operations
- Errors and warnings
- Performance metrics

## CORS Configuration

CORS is enabled with "AllowAll" policy for development. For production:
- Restrict allowed origins
- Specify allowed methods
- Configure credentials policy

## Common EF Commands

```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# List all migrations
dotnet ef migrations list

# Drop database
dotnet ef database drop

# Generate SQL script
dotnet ef migrations script
```

## Sample Data

The database is seeded with 4 sample students:
1. Alice Johnson - Computer Science
2. Bob Williams - Data Science
3. Carol Martinez - Software Engineering
4. David Brown - Artificial Intelligence

## Security Considerations

For production deployment:
- ✅ Enable authentication and authorization
- ✅ Use HTTPS only
- ✅ Implement rate limiting
- ✅ Add input sanitization
- ✅ Configure proper CORS policy
- ✅ Use connection string encryption
- ✅ Implement API versioning
- ✅ Add request/response compression

## Future Enhancements

- [ ] Pagination for large datasets
- [ ] Filtering and sorting options
- [ ] JWT authentication
- [ ] API versioning
- [ ] Caching with Redis
- [ ] Performance monitoring
- [ ] Audit logging
- [ ] Soft delete functionality

## Author

Created as a demonstration of RESTful API design with ASP.NET Core, Entity Framework Core, and Repository Design Pattern.

---

**API Documentation**: Available at root URL when application is running
**Swagger UI**: `https://localhost:5001/swagger`
