using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core_MVC_EF.Data;
using Core_MVC_EF.Models;

namespace Core_MVC_EF.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Students
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString)
                                       || s.Email.Contains(searchString)
                                       || s.Course.Contains(searchString));
            }

            var studentList = await students.OrderBy(s => s.Name).ToListAsync();
            ViewBag.TotalStudents = studentList.Count;
            
            return View(studentList);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Course,EnrollmentDate,Phone,Address,IsActive")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    student.CreatedDate = DateTime.Now;
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Email", "A student with this email already exists.");
                }
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Course,EnrollmentDate,Phone,Address,IsActive,CreatedDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("Email", "A student with this email already exists.");
                }
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
