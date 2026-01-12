using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using REST_API.DTOs;
using REST_API.Models;
using REST_API.Repository;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            IStudentRepository repository,
            IMapper mapper,
            ILogger<StudentsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns>List of all students</returns>
        /// <response code="200">Returns the list of students</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetAllStudents()
        {
            _logger.LogInformation("Getting all students");
            
            var students = await _repository.GetAllAsync();
            var studentDtos = _mapper.Map<IEnumerable<StudentReadDto>>(students);
            
            return Ok(new
            {
                success = true,
                count = studentDtos.Count(),
                data = studentDtos
            });
        }

        /// <summary>
        /// Get a specific student by ID
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Student details</returns>
        /// <response code="200">Returns the student</response>
        /// <response code="404">Student not found</response>
        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentReadDto>> GetStudentById(int id)
        {
            _logger.LogInformation("Getting student with ID: {Id}", id);
            
            var student = await _repository.GetByIdAsync(id);
            
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
                return NotFound(new { success = false, message = $"Student with ID {id} not found" });
            }

            var studentDto = _mapper.Map<StudentReadDto>(student);
            return Ok(new { success = true, data = studentDto });
        }

        /// <summary>
        /// Create a new student
        /// </summary>
        /// <param name="studentCreateDto">Student data</param>
        /// <returns>Created student</returns>
        /// <response code="201">Student created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="409">Email already exists</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<StudentReadDto>> CreateStudent([FromBody] StudentCreateDto studentCreateDto)
        {
            _logger.LogInformation("Creating new student: {Email}", studentCreateDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            // Check if email already exists
            if (await _repository.EmailExistsAsync(studentCreateDto.Email))
            {
                _logger.LogWarning("Email already exists: {Email}", studentCreateDto.Email);
                return Conflict(new { success = false, message = "A student with this email already exists" });
            }

            var student = _mapper.Map<Student>(studentCreateDto);
            var createdStudent = await _repository.AddAsync(student);
            var studentDto = _mapper.Map<StudentReadDto>(createdStudent);

            _logger.LogInformation("Student created with ID: {Id}", createdStudent.Id);

            return CreatedAtRoute(
                nameof(GetStudentById),
                new { id = studentDto.Id },
                new { success = true, message = "Student created successfully", data = studentDto }
            );
        }

        /// <summary>
        /// Update an existing student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studentUpdateDto">Updated student data</param>
        /// <returns>Updated student</returns>
        /// <response code="200">Student updated successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Student not found</response>
        /// <response code="409">Email already exists</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<StudentReadDto>> UpdateStudent(int id, [FromBody] StudentUpdateDto studentUpdateDto)
        {
            _logger.LogInformation("Updating student with ID: {Id}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState });
            }

            var existingStudent = await _repository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found for update", id);
                return NotFound(new { success = false, message = $"Student with ID {id} not found" });
            }

            // Check if email already exists for another student
            if (await _repository.EmailExistsAsync(studentUpdateDto.Email, id))
            {
                _logger.LogWarning("Email already exists: {Email}", studentUpdateDto.Email);
                return Conflict(new { success = false, message = "A student with this email already exists" });
            }

            _mapper.Map(studentUpdateDto, existingStudent);
            existingStudent.Id = id; // Ensure ID doesn't change
            
            var updatedStudent = await _repository.UpdateAsync(existingStudent);
            var studentDto = _mapper.Map<StudentReadDto>(updatedStudent);

            _logger.LogInformation("Student updated with ID: {Id}", id);

            return Ok(new { success = true, message = "Student updated successfully", data = studentDto });
        }

        /// <summary>
        /// Delete a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>Deletion confirmation</returns>
        /// <response code="200">Student deleted successfully</response>
        /// <response code="404">Student not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            _logger.LogInformation("Deleting student with ID: {Id}", id);

            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found for deletion", id);
                return NotFound(new { success = false, message = $"Student with ID {id} not found" });
            }

            var result = await _repository.DeleteAsync(id);
            
            if (result)
            {
                _logger.LogInformation("Student deleted with ID: {Id}", id);
                return Ok(new { success = true, message = "Student deleted successfully" });
            }

            return StatusCode(500, new { success = false, message = "An error occurred while deleting the student" });
        }

        /// <summary>
        /// Search students by name, email, or course
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of matching students</returns>
        /// <response code="200">Returns matching students</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> SearchStudents([FromQuery] string searchTerm)
        {
            _logger.LogInformation("Searching students with term: {SearchTerm}", searchTerm);
            
            var students = await _repository.SearchStudentsAsync(searchTerm);
            var studentDtos = _mapper.Map<IEnumerable<StudentReadDto>>(students);
            
            return Ok(new
            {
                success = true,
                searchTerm = searchTerm,
                count = studentDtos.Count(),
                data = studentDtos
            });
        }

        /// <summary>
        /// Get students by course
        /// </summary>
        /// <param name="course">Course name</param>
        /// <returns>List of students in the course</returns>
        /// <response code="200">Returns students in the course</response>
        [HttpGet("course/{course}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetStudentsByCourse(string course)
        {
            _logger.LogInformation("Getting students by course: {Course}", course);
            
            var students = await _repository.GetStudentsByCourseAsync(course);
            var studentDtos = _mapper.Map<IEnumerable<StudentReadDto>>(students);
            
            return Ok(new
            {
                success = true,
                course = course,
                count = studentDtos.Count(),
                data = studentDtos
            });
        }

        /// <summary>
        /// Get only active students
        /// </summary>
        /// <returns>List of active students</returns>
        /// <response code="200">Returns active students</response>
        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentReadDto>>> GetActiveStudents()
        {
            _logger.LogInformation("Getting active students");
            
            var students = await _repository.GetActiveStudentsAsync();
            var studentDtos = _mapper.Map<IEnumerable<StudentReadDto>>(students);
            
            return Ok(new
            {
                success = true,
                count = studentDtos.Count(),
                data = studentDtos
            });
        }

        /// <summary>
        /// Get total count of students
        /// </summary>
        /// <returns>Total count</returns>
        /// <response code="200">Returns the count</response>
        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            _logger.LogInformation("Getting total student count");
            
            var count = await _repository.GetTotalCountAsync();
            
            return Ok(new { success = true, totalCount = count });
        }
    }
}
