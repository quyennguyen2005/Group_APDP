using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.Services.Auth;
using WebApp.UnitOfWork;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class CoursesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _dbContext;
    private readonly AuthService _authService;

    public CoursesController(IUnitOfWork unitOfWork, ApplicationDbContext dbContext, AuthService authService)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var items = new List<CourseListItemViewModel>();
        foreach (var course in courses)
        {
            var enrolled = await _unitOfWork.Enrollments.GetByCourseAsync(course.Id);
            items.Add(new CourseListItemViewModel
            {
                Course = course,
                EnrolledCount = enrolled.Count
            });
        }

        var ordered = items.OrderBy(c => c.Course.CourseCode).ToList();
        return View(ordered);
    }

    public async Task<IActionResult> Details(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        var enrollments = await _unitOfWork.Enrollments.GetByCourseAsync(id);
        var students = await _unitOfWork.Students.GetAllAsync();

        // Get current user's student ID if they are a student
        int? currentStudentId = null;
        var username = HttpContext.Session.GetString("Username");
        var role = HttpContext.Session.GetString("Role");
        
        if (!string.IsNullOrEmpty(username) && role == "Student")
        {
            var userAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (userAccount == null)
            {
                var authUser = await _authService.GetUserByUsernameAsync(username);
                if (authUser != null && authUser.StudentId.HasValue)
                {
                    currentStudentId = authUser.StudentId.Value;
                }
            }
            else if (userAccount.StudentId.HasValue)
            {
                currentStudentId = userAccount.StudentId.Value;
            }
        }

        var enrolledStudents = students
            .Where(s => enrollments.Any(e => e.StudentId == s.Id))
            .OrderBy(s => s.FullName)
            .ToList();
        
        var availableStudents = students
            .Where(s => enrollments.All(e => e.StudentId != s.Id))
            .OrderBy(s => s.FullName)
            .ToList();

        var vm = new CourseDetailsViewModel
        {
            Course = course,
            EnrolledStudents = enrolledStudents,
            AvailableStudents = availableStudents
        };

        ViewBag.CurrentStudentId = currentStudentId;
        ViewBag.IsEnrolled = currentStudentId.HasValue && enrollments.Any(e => e.StudentId == currentStudentId.Value);
        ViewBag.CanManage = AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session);

        return View(vm);
    }

    public IActionResult Create()
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to add courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }
        return View(new Course
        {
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to add courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(course);
        }

        // Check for duplicate CourseCode
        var existingByCode = (await _unitOfWork.Courses.GetAllAsync())
            .FirstOrDefault(c => c.CourseCode.Equals(course.CourseCode, StringComparison.OrdinalIgnoreCase));
        
        if (existingByCode != null)
        {
            ModelState.AddModelError(nameof(Course.CourseCode), "A course with this course code already exists.");
            return View(course);
        }

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();
        TempData["SuccessMessage"] = "Course created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to edit courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }

        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to edit courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }

        if (id != course.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(course);
        }

        // Check for duplicate CourseCode (excluding current course)
        var existingByCode = (await _unitOfWork.Courses.GetAllAsync())
            .FirstOrDefault(c => c.Id != course.Id && 
                                c.CourseCode.Equals(course.CourseCode, StringComparison.OrdinalIgnoreCase));
        
        if (existingByCode != null)
        {
            ModelState.AddModelError(nameof(Course.CourseCode), "A course with this course code already exists.");
            return View(course);
        }

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();
        TempData["SuccessMessage"] = "Course updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to delete courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }

        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session))
        {
            TempData["ErrorMessage"] = "You do not have permission to delete courses. Only Admin and Instructors have this permission.";
            return RedirectToAction(nameof(Index));
        }

        await _unitOfWork.Courses.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignStudent(int courseId, int selectedStudentId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound();
        }

        var username = HttpContext.Session.GetString("Username");
        var role = HttpContext.Session.GetString("Role");
        var isAdminOrInstructor = AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session);

        // Get current user's student ID if they are a student
        int? currentStudentId = null;
        if (!string.IsNullOrEmpty(username) && role == "Student")
        {
            var userAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (userAccount == null)
            {
                var authUser = await _authService.GetUserByUsernameAsync(username);
                if (authUser != null && authUser.StudentId.HasValue)
                {
                    currentStudentId = authUser.StudentId.Value;
                }
            }
            else if (userAccount.StudentId.HasValue)
            {
                currentStudentId = userAccount.StudentId.Value;
            }
        }

        // If user is a student, they can only enroll themselves
        if (role == "Student" && !isAdminOrInstructor)
        {
            if (!currentStudentId.HasValue)
            {
                TempData["CourseMessage"] = "Student account not found.";
                return RedirectToAction(nameof(Details), new { id = courseId });
            }

            if (selectedStudentId != currentStudentId.Value)
            {
                TempData["CourseMessage"] = "You can only enroll yourself in courses.";
                return RedirectToAction(nameof(Details), new { id = courseId });
            }
        }

        if (selectedStudentId == 0)
        {
            TempData["CourseMessage"] = "Please select a student.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Check if already enrolled
        var existingEnrollments = await _unitOfWork.Enrollments.GetByCourseAsync(courseId);
        if (existingEnrollments.Any(e => e.StudentId == selectedStudentId))
        {
            TempData["CourseMessage"] = "Student is already enrolled in this course.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Check course capacity
        if (existingEnrollments.Count >= course.MaxStudents)
        {
            TempData["CourseMessage"] = "Course is full. Cannot enroll more students.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        await _unitOfWork.Enrollments.AddAsync(new Enrollment
        {
            CourseId = courseId,
            StudentId = selectedStudentId
        });
        await _unitOfWork.SaveChangesAsync();
        TempData["CourseMessage"] = "Student has been assigned to the course.";
        return RedirectToAction(nameof(Details), new { id = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnrollSelf(int courseId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
        if (course == null)
        {
            return NotFound();
        }

        var username = HttpContext.Session.GetString("Username");
        var role = HttpContext.Session.GetString("Role");

        if (role != "Student")
        {
            TempData["CourseMessage"] = "Only students can enroll in courses.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Get current user's student ID
        int? currentStudentId = null;
        if (!string.IsNullOrEmpty(username))
        {
            var userAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (userAccount == null)
            {
                var authUser = await _authService.GetUserByUsernameAsync(username);
                if (authUser != null && authUser.StudentId.HasValue)
                {
                    currentStudentId = authUser.StudentId.Value;
                }
            }
            else if (userAccount.StudentId.HasValue)
            {
                currentStudentId = userAccount.StudentId.Value;
            }
        }

        if (!currentStudentId.HasValue)
        {
            TempData["CourseMessage"] = "Student account not found. Please contact administrator.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Check if already enrolled
        var existingEnrollments = await _unitOfWork.Enrollments.GetByCourseAsync(courseId);
        if (existingEnrollments.Any(e => e.StudentId == currentStudentId.Value))
        {
            TempData["CourseMessage"] = "You are already enrolled in this course.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Check course capacity
        if (existingEnrollments.Count >= course.MaxStudents)
        {
            TempData["CourseMessage"] = "Course is full. Cannot enroll.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        await _unitOfWork.Enrollments.AddAsync(new Enrollment
        {
            CourseId = courseId,
            StudentId = currentStudentId.Value
        });
        await _unitOfWork.SaveChangesAsync();
        TempData["CourseMessage"] = "You have successfully enrolled in this course.";
        return RedirectToAction(nameof(Details), new { id = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveStudent(int courseId, int studentId)
    {
        var username = HttpContext.Session.GetString("Username");
        var role = HttpContext.Session.GetString("Role");
        var isAdminOrInstructor = AuthorizationHelper.IsAdminOrInstructor(HttpContext.Session);

        // Get current user's student ID if they are a student
        int? currentStudentId = null;
        if (!string.IsNullOrEmpty(username) && role == "Student")
        {
            var userAccount = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (userAccount == null)
            {
                var authUser = await _authService.GetUserByUsernameAsync(username);
                if (authUser != null && authUser.StudentId.HasValue)
                {
                    currentStudentId = authUser.StudentId.Value;
                }
            }
            else if (userAccount.StudentId.HasValue)
            {
                currentStudentId = userAccount.StudentId.Value;
            }
        }

        // If user is a student, they can only remove themselves
        if (role == "Student" && !isAdminOrInstructor)
        {
            if (!currentStudentId.HasValue)
            {
                TempData["CourseMessage"] = "Student account not found.";
                return RedirectToAction(nameof(Details), new { id = courseId });
            }

            if (studentId != currentStudentId.Value)
            {
                TempData["CourseMessage"] = "You can only remove yourself from courses.";
                return RedirectToAction(nameof(Details), new { id = courseId });
            }
        }

        var enrollments = await _unitOfWork.Enrollments.GetByCourseAsync(courseId);
        var target = enrollments.FirstOrDefault(e => e.StudentId == studentId);
        if (target != null)
        {
            await _unitOfWork.Enrollments.RemoveAsync(target.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        
        if (role == "Student" && currentStudentId.HasValue && studentId == currentStudentId.Value)
        {
            TempData["CourseMessage"] = "You have been removed from the course.";
        }
        else
        {
            TempData["CourseMessage"] = "Student has been removed from the course.";
        }
        
        return RedirectToAction(nameof(Details), new { id = courseId });
    }
}

