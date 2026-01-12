using Microsoft.AspNetCore.Mvc;
using API_Consumer.Models;
using API_Consumer.Services;

namespace API_Consumer.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IStudentApiService _apiService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentApiService apiService, ILogger<StudentsController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchTerm)
        {
            try
            {
                IEnumerable<Student> students;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    ViewData["SearchTerm"] = searchTerm;
                    students = await _apiService.SearchStudentsAsync(searchTerm);
                    TempData["InfoMessage"] = $"Found {students.Count()} student(s) matching '{searchTerm}'";
                }
                else
                {
                    students = await _apiService.GetAllStudentsAsync();
                }

                var totalCount = await _apiService.GetTotalCountAsync();
                ViewBag.TotalCount = totalCount;

                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading students");
                TempData["ErrorMessage"] = "Error loading students. Please ensure the API is running.";
                return View(Enumerable.Empty<Student>());
            }
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var student = await _apiService.GetStudentByIdAsync(id);
                
                if (student == null)
                {
                    TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student details");
                TempData["ErrorMessage"] = "Error loading student details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(student);
                }

                var createdStudent = await _apiService.CreateStudentAsync(student);

                if (createdStudent != null)
                {
                    TempData["SuccessMessage"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Failed to create student. Email might already exist.";
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating student");
                TempData["ErrorMessage"] = "Error creating student.";
                return View(student);
            }
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var student = await _apiService.GetStudentByIdAsync(id);
                
                if (student == null)
                {
                    TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student for edit");
                TempData["ErrorMessage"] = "Error loading student.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(student);
                }

                var success = await _apiService.UpdateStudentAsync(id, student);

                if (success)
                {
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Failed to update student. Email might already exist.";
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating student");
                TempData["ErrorMessage"] = "Error updating student.";
                return View(student);
            }
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var student = await _apiService.GetStudentByIdAsync(id);
                
                if (student == null)
                {
                    TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading student for delete");
                TempData["ErrorMessage"] = "Error loading student.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _apiService.DeleteStudentAsync(id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Student deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete student.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting student");
                TempData["ErrorMessage"] = "Error deleting student.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Students/ByCourse
        public async Task<IActionResult> ByCourse(string course)
        {
            try
            {
                if (string.IsNullOrEmpty(course))
                {
                    return RedirectToAction(nameof(Index));
                }

                var students = await _apiService.GetStudentsByCourseAsync(course);
                ViewBag.Course = course;
                ViewBag.Count = students.Count();

                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading students by course");
                TempData["ErrorMessage"] = "Error loading students by course.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Students/Active
        public async Task<IActionResult> Active()
        {
            try
            {
                var students = await _apiService.GetActiveStudentsAsync();
                ViewBag.Count = students.Count();

                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading active students");
                TempData["ErrorMessage"] = "Error loading active students.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
