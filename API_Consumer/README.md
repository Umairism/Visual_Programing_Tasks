# API Consumer - LINQ to JSON with Newtonsoft.Json

A complete ASP.NET Core MVC application that consumes REST API using **LINQ to JSON** with the **Newtonsoft.Json** package.

## Features

- ✅ **REST API Consumption**: HttpClient to consume Student REST API
- ✅ **LINQ to JSON**: Extensive use of JObject, JArray, and LINQ queries
- ✅ **Newtonsoft.Json**: JSON parsing and manipulation
- ✅ **Full CRUD Operations**: Create, Read, Update, Delete via API
- ✅ **Advanced Filtering**: Search, filter by course, active students
- ✅ **Error Handling**: Graceful handling of API errors
- ✅ **Modern UI**: Responsive Bootstrap 5 design
- ✅ **Dependency Injection**: HttpClient factory pattern

## Technology Stack

- **ASP.NET Core 8.0 MVC** - Web framework
- **Newtonsoft.Json 13.0.3** - JSON manipulation
- **HttpClient** - REST API communication
- **Bootstrap 5** - UI framework
- **Font Awesome 6** - Icons

## Project Structure

```
API_Consumer/
├── Controllers/
│   └── StudentsController.cs      # MVC controller with API calls
├── Models/
│   └── Student.cs                 # Student data model
├── Services/
│   ├── IStudentApiService.cs      # Service interface
│   └── StudentApiService.cs       # API service with LINQ to JSON
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml         # Main layout
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Students/
│   │   ├── Index.cshtml           # List all students
│   │   ├── Details.cshtml         # View details
│   │   ├── Create.cshtml          # Add new student
│   │   ├── Edit.cshtml            # Update student
│   │   ├── Delete.cshtml          # Delete confirmation
│   │   ├── ByCourse.cshtml        # Filter by course
│   │   └── Active.cshtml          # Active students only
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   └── css/
│       └── site.css               # Custom styles
├── appsettings.json               # Configuration (API URL)
├── Program.cs                     # Application startup
└── API_Consumer.csproj           # Project file
```

## LINQ to JSON Features Demonstrated

### 1. **JObject.Parse()** - Parse JSON Strings
```csharp
var jsonObject = JObject.Parse(jsonString);
var studentToken = jsonObject["data"];
```

### 2. **JArray** - Handle JSON Arrays
```csharp
var studentsArray = jsonObject["data"] as JArray;
var students = studentsArray
    .Select(token => token.ToObject<Student>())
    .ToList();
```

### 3. **LINQ Queries on JSON**
```csharp
var students = studentsArray
    .Where(token => token["IsActive"]?.Value<bool>() == true)
    .Select(token => token.ToObject<Student>())
    .OrderBy(s => s.Name)
    .ToList();
```

### 4. **JToken.ToObject<T>()** - Convert to Strongly-Typed Objects
```csharp
var student = studentToken.ToObject<Student>();
```

### 5. **Dynamic JSON Creation with JObject**
```csharp
var studentJson = new JObject
{
    ["Name"] = student.Name,
    ["Email"] = student.Email,
    ["Course"] = student.Course,
    ["EnrollmentDate"] = student.EnrollmentDate.ToString("yyyy-MM-dd"),
    ["IsActive"] = student.IsActive
};
```

### 6. **Value Extraction**
```csharp
var count = jsonObject["totalCount"]?.Value<int>() ?? 0;
var isActive = token["IsActive"]?.Value<bool>() ?? false;
var courseName = token["Course"]?.Value<string>();
```

### 7. **Complex LINQ to JSON Queries**
```csharp
var students = studentsArray
    .Where(token => 
    {
        var isActive = token["IsActive"]?.Value<bool>() ?? false;
        var enrollmentDate = token["EnrollmentDate"]?.Value<DateTime>() ?? DateTime.MinValue;
        return isActive && enrollmentDate >= DateTime.Now.AddYears(-5);
    })
    .Select(token => token.ToObject<Student>())
    .OrderByDescending(s => s.EnrollmentDate)
    .ToList();
```

## API Endpoints Consumed

The application consumes the following REST API endpoints:

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/students` | Get all students |
| GET | `/api/students/{id}` | Get student by ID |
| POST | `/api/students` | Create new student |
| PUT | `/api/students/{id}` | Update student |
| DELETE | `/api/students/{id}` | Delete student |
| GET | `/api/students/search?searchTerm=...` | Search students |
| GET | `/api/students/course/{course}` | Get students by course |
| GET | `/api/students/active` | Get active students |
| GET | `/api/students/count` | Get total count |

## Setup Instructions

### Prerequisites
1. .NET 6.0 SDK or later
2. REST API project must be running on `https://localhost:5001`

### Installation Steps

1. **Navigate to project directory:**
```bash
cd "e:\Visual Programing Practice\Taska\API_Consumer"
```

2. **Restore NuGet packages:**
```bash
dotnet restore
```

3. **Ensure REST API is running:**
```bash
# In another terminal, start the REST API
cd "..\REST_API"
dotnet run
```

4. **Run the consumer application:**
```bash
dotnet run
```

5. **Access the application:**
Open browser and navigate to: `https://localhost:7001` (or the port shown in terminal)

## Configuration

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:5001/api",
    "Timeout": 30
  }
}
```

To change the API endpoint, modify the `BaseUrl` in appsettings.json.

## How It Works

### Service Layer (StudentApiService)

The service layer handles all API communication using HttpClient and LINQ to JSON:

1. **GET Request with LINQ to JSON:**
```csharp
var response = await _httpClient.GetAsync("/students");
var jsonString = await response.Content.ReadAsStringAsync();
var jsonObject = JObject.Parse(jsonString);
var studentsArray = jsonObject["data"] as JArray;
var students = studentsArray
    .Select(token => token.ToObject<Student>())
    .ToList();
```

2. **POST Request with JObject:**
```csharp
var studentJson = new JObject
{
    ["Name"] = student.Name,
    ["Email"] = student.Email,
    // ... more properties
};
var content = new StringContent(studentJson.ToString(), Encoding.UTF8, "application/json");
var response = await _httpClient.PostAsync("/students", content);
```

3. **Complex LINQ Queries:**
```csharp
var students = studentsArray
    .Where(token => token["IsActive"]?.Value<bool>() == true)
    .Select(token => new
    {
        Student = token.ToObject<Student>(),
        CourseName = token["Course"]?.Value<string>()
    })
    .Where(x => x.Student != null)
    .Select(x => x.Student!)
    .ToList();
```

### Controller Layer

Controllers call the service methods and pass data to views:

```csharp
public async Task<IActionResult> Index(string searchTerm)
{
    var students = string.IsNullOrEmpty(searchTerm)
        ? await _apiService.GetAllStudentsAsync()
        : await _apiService.SearchStudentsAsync(searchTerm);
    
    return View(students);
}
```

### View Layer

Razor views display the data with Bootstrap 5 styling.

## LINQ to JSON Examples in Code

### Example 1: Filtering with LINQ
```csharp
// In GetActiveStudentsAsync()
var students = studentsArray
    .Where(token => 
    {
        var isActive = token["IsActive"]?.Value<bool>() ?? false;
        var enrollmentDate = token["EnrollmentDate"]?.Value<DateTime>() ?? DateTime.MinValue;
        return isActive && enrollmentDate >= DateTime.Now.AddYears(-5);
    })
    .Select(token => token.ToObject<Student>())
    .Where(s => s != null)
    .Cast<Student>()
    .OrderByDescending(s => s.EnrollmentDate)
    .ToList();
```

### Example 2: Projection with LINQ
```csharp
// In GetStudentsByCourseAsync()
var students = studentsArray
    .Select(token => new
    {
        Student = token.ToObject<Student>(),
        CourseName = token["Course"]?.Value<string>()
    })
    .Where(x => x.Student != null && !string.IsNullOrEmpty(x.CourseName))
    .Select(x => x.Student!)
    .ToList();
```

### Example 3: Creating JSON Dynamically
```csharp
// In CreateStudentAsync()
var studentJson = new JObject
{
    ["Name"] = student.Name,
    ["Email"] = student.Email,
    ["Course"] = student.Course,
    ["EnrollmentDate"] = student.EnrollmentDate.ToString("yyyy-MM-dd"),
    ["Phone"] = student.Phone,
    ["Address"] = student.Address ?? string.Empty,
    ["IsActive"] = student.IsActive
};
```

## Pages and Features

### 1. Index (Students List)
- Display all students in a table
- Search functionality
- Link to filter by course
- Shows total count
- CRUD action buttons

### 2. Details
- View complete student information
- Fetched from API using LINQ to JSON
- Edit and Delete buttons

### 3. Create
- Form to add new student
- Data sent as JObject to API
- Client and server validation

### 4. Edit
- Form to update student information
- Pre-populated with existing data
- Sent as JObject via PUT request

### 5. Delete
- Confirmation page
- Shows student details before deletion
- DELETE request to API

### 6. By Course
- Filter students by selected course
- Uses LINQ to JSON queries
- Clickable course badges on Index

### 7. Active Students
- Shows only active students
- Complex LINQ to JSON filtering
- Filters students enrolled within last 5 years

## Error Handling

The application handles various error scenarios:

- **API Not Running**: Displays friendly error message
- **Network Errors**: Caught and logged
- **Invalid Data**: Validation errors shown
- **404 Not Found**: Redirects with error message
- **409 Conflict**: Email already exists handling

## Logging

Comprehensive logging throughout the application:

```csharp
_logger.LogInformation("Fetching all students from API");
_logger.LogWarning("Student with ID: {Id} not found", id);
_logger.LogError(ex, "Error fetching students");
```

## Testing the Application

### Step 1: Start the REST API
```bash
cd "e:\Visual Programing Practice\Taska\REST_API"
dotnet run
```
Wait for: `Now listening on: https://localhost:5001`

### Step 2: Start the Consumer Application
```bash
cd "e:\Visual Programing Practice\Taska\API_Consumer"
dotnet run
```

### Step 3: Test Features
1. View all students (Index page)
2. Search for students
3. Create a new student
4. Edit existing student
5. View student details
6. Filter by course
7. View active students only
8. Delete a student

## Key Benefits of LINQ to JSON

1. **Flexibility**: Query JSON without deserializing to objects first
2. **Performance**: Process only needed data
3. **Dynamic Queries**: Build complex queries on JSON structures
4. **Transformation**: Easy JSON manipulation and transformation
5. **Debugging**: Inspect JSON structure during development
6. **Mixed Approach**: Combine LINQ queries with strong typing

## Comparison: LINQ to JSON vs Direct Deserialization

### Direct Deserialization (Traditional):
```csharp
var response = JsonConvert.DeserializeObject<ApiResponse<Student>>(jsonString);
var students = response.Data;
```

### LINQ to JSON (This Project):
```csharp
var jsonObject = JObject.Parse(jsonString);
var students = (jsonObject["data"] as JArray)
    ?.Select(token => token.ToObject<Student>())
    .Where(s => s.IsActive)  // Filter in LINQ!
    .ToList();
```

**Advantages of LINQ to JSON:**
- Query before deserialization
- Handle dynamic JSON structures
- Partial deserialization
- Transform on-the-fly

## Dependencies

```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

## Common Issues and Solutions

### Issue: "Cannot connect to API"
**Solution**: Ensure REST API is running on `https://localhost:5001`

### Issue: "SSL Certificate Error"
**Solution**: Run in development environment or configure SSL properly

### Issue: "No students displayed"
**Solution**: 
1. Check if REST API has data
2. Check browser console for errors
3. Verify API base URL in appsettings.json

## Future Enhancements

- [ ] Add pagination
- [ ] Implement caching
- [ ] Add authentication
- [ ] Export to Excel/PDF
- [ ] Bulk operations
- [ ] Real-time updates with SignalR
- [ ] Offline mode with local storage

## Author

Created as a demonstration of ASP.NET Core MVC consuming REST APIs with LINQ to JSON using Newtonsoft.Json package.

---

**Note**: This application requires the REST_API project to be running. Both projects work together to demonstrate API consumption with LINQ to JSON capabilities.

## Summary

This project showcases:
- ✅ REST API consumption with HttpClient
- ✅ LINQ to JSON queries (JObject, JArray)
- ✅ JSON parsing and manipulation with Newtonsoft.Json
- ✅ Complex filtering and transformation
- ✅ Full CRUD operations via API
- ✅ Modern ASP.NET Core MVC architecture
- ✅ Dependency injection with HttpClient factory
- ✅ Error handling and logging
- ✅ Responsive UI with Bootstrap 5
