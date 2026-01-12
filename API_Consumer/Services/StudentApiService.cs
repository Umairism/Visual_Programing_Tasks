using API_Consumer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace API_Consumer.Services
{
    /// <summary>
    /// Service to consume Student REST API using LINQ to JSON with Newtonsoft.Json
    /// Demonstrates LINQ to JSON queries and manipulation
    /// </summary>
    public class StudentApiService : IStudentApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StudentApiService> _logger;
        private readonly string _baseUrl;

        public StudentApiService(HttpClient httpClient, IConfiguration configuration, ILogger<StudentApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001/api";
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // GET: Get all students using LINQ to JSON
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all students from API");
                
                var response = await _httpClient.GetAsync("/students");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse JSON using LINQ to JSON (JObject)
                var jsonObject = JObject.Parse(jsonString);
                
                // Use LINQ to JSON to query the data array
                var studentsArray = jsonObject["data"] as JArray;
                
                if (studentsArray == null)
                    return Enumerable.Empty<Student>();

                // Convert JArray to Student objects using LINQ
                var students = studentsArray
                    .Select(token => token.ToObject<Student>())
                    .Where(s => s != null)
                    .Cast<Student>()
                    .ToList();

                _logger.LogInformation("Successfully fetched {Count} students", students.Count);
                return students;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all students");
                return Enumerable.Empty<Student>();
            }
        }

        // GET: Get student by ID using LINQ to JSON
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching student with ID: {Id}", id);
                
                var response = await _httpClient.GetAsync($"/students/{id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Student with ID {Id} not found", id);
                    return null;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse using LINQ to JSON
                var jsonObject = JObject.Parse(jsonString);
                
                // Extract the data property using LINQ to JSON
                var studentToken = jsonObject["data"];
                
                if (studentToken == null)
                    return null;

                // Convert JToken to Student object
                var student = studentToken.ToObject<Student>();
                
                _logger.LogInformation("Successfully fetched student: {Name}", student?.Name);
                return student;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching student with ID: {Id}", id);
                return null;
            }
        }

        // POST: Create new student using LINQ to JSON
        public async Task<Student?> CreateStudentAsync(Student student)
        {
            try
            {
                _logger.LogInformation("Creating new student: {Email}", student.Email);

                // Create JObject manually to demonstrate LINQ to JSON manipulation
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

                var content = new StringContent(
                    studentJson.ToString(),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("/students", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to create student. Status: {Status}, Error: {Error}", 
                        response.StatusCode, errorContent);
                    return null;
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse response using LINQ to JSON
                var responseObject = JObject.Parse(jsonString);
                var createdStudent = responseObject["data"]?.ToObject<Student>();

                _logger.LogInformation("Successfully created student with ID: {Id}", createdStudent?.Id);
                return createdStudent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                return null;
            }
        }

        // PUT: Update student using LINQ to JSON
        public async Task<bool> UpdateStudentAsync(int id, Student student)
        {
            try
            {
                _logger.LogInformation("Updating student with ID: {Id}", id);

                // Use LINQ to JSON to build update payload
                var updateJson = new JObject
                {
                    ["Name"] = student.Name,
                    ["Email"] = student.Email,
                    ["Course"] = student.Course,
                    ["EnrollmentDate"] = student.EnrollmentDate.ToString("yyyy-MM-dd"),
                    ["Phone"] = student.Phone,
                    ["Address"] = student.Address ?? string.Empty,
                    ["IsActive"] = student.IsActive
                };

                var content = new StringContent(
                    updateJson.ToString(),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PutAsync($"/students/{id}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully updated student with ID: {Id}", id);
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to update student. Status: {Status}, Error: {Error}", 
                    response.StatusCode, errorContent);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student with ID: {Id}", id);
                return false;
            }
        }

        // DELETE: Delete student
        public async Task<bool> DeleteStudentAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting student with ID: {Id}", id);
                
                var response = await _httpClient.DeleteAsync($"/students/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted student with ID: {Id}", id);
                    return true;
                }

                _logger.LogWarning("Failed to delete student with ID: {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student with ID: {Id}", id);
                return false;
            }
        }

        // SEARCH: Search students using LINQ to JSON
        public async Task<IEnumerable<Student>> SearchStudentsAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching students with term: {SearchTerm}", searchTerm);
                
                var response = await _httpClient.GetAsync($"/students/search?searchTerm={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);
                
                // Use LINQ to JSON to extract and filter data
                var studentsArray = jsonObject["data"] as JArray;
                
                if (studentsArray == null)
                    return Enumerable.Empty<Student>();

                // Demonstrate LINQ to JSON query - filter active students from results
                var students = studentsArray
                    .Where(token => token["IsActive"]?.Value<bool>() == true)
                    .Select(token => token.ToObject<Student>())
                    .Where(s => s != null)
                    .Cast<Student>()
                    .OrderBy(s => s.Name)
                    .ToList();

                _logger.LogInformation("Found {Count} students matching '{SearchTerm}'", students.Count, searchTerm);
                return students;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching students");
                return Enumerable.Empty<Student>();
            }
        }

        // GET: Get students by course using LINQ to JSON
        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(string course)
        {
            try
            {
                _logger.LogInformation("Fetching students for course: {Course}", course);
                
                var response = await _httpClient.GetAsync($"/students/course/{Uri.EscapeDataString(course)}");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonString);
                
                var studentsArray = jsonObject["data"] as JArray;
                
                if (studentsArray == null)
                    return Enumerable.Empty<Student>();

                // Use LINQ to JSON to transform and query
                var students = studentsArray
                    .Select(token => new
                    {
                        Student = token.ToObject<Student>(),
                        CourseName = token["Course"]?.Value<string>()
                    })
                    .Where(x => x.Student != null && !string.IsNullOrEmpty(x.CourseName))
                    .Select(x => x.Student!)
                    .ToList();

                _logger.LogInformation("Found {Count} students in course: {Course}", students.Count, course);
                return students;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching students by course");
                return Enumerable.Empty<Student>();
            }
        }

        // GET: Get active students using LINQ to JSON
        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching active students");
                
                var response = await _httpClient.GetAsync("/students/active");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse and query using LINQ to JSON
                var jsonObject = JObject.Parse(jsonString);
                var studentsArray = jsonObject["data"] as JArray;
                
                if (studentsArray == null)
                    return Enumerable.Empty<Student>();

                // Demonstrate advanced LINQ to JSON - grouping and filtering
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

                _logger.LogInformation("Found {Count} active students", students.Count);
                return students;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active students");
                return Enumerable.Empty<Student>();
            }
        }

        // GET: Get total count using LINQ to JSON
        public async Task<int> GetTotalCountAsync()
        {
            try
            {
                _logger.LogInformation("Fetching total student count");
                
                var response = await _httpClient.GetAsync("/students/count");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                
                // Parse using LINQ to JSON
                var jsonObject = JObject.Parse(jsonString);
                
                // Extract count value using LINQ to JSON query
                var count = jsonObject["totalCount"]?.Value<int>() ?? 0;

                _logger.LogInformation("Total student count: {Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching total count");
                return 0;
            }
        }
    }
}
