using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.UnitOfWork;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class CoursesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoursesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        return View(vm);
    }

    public IActionResult Create()
    {
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
        if (!ModelState.IsValid)
        {
            return View(course);
        }

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
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
        if (id != course.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(course);
        }

        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
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

        if (selectedStudentId == 0)
        {
            TempData["CourseMessage"] = "Vui lòng chọn một sinh viên.";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        await _unitOfWork.Enrollments.AddAsync(new Enrollment
        {
            CourseId = courseId,
            StudentId = selectedStudentId
        });
        await _unitOfWork.SaveChangesAsync();
        TempData["CourseMessage"] = "Đã phân công sinh viên vào khoá học.";
        return RedirectToAction(nameof(Details), new { id = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveStudent(int courseId, int studentId)
    {
        var enrollments = await _unitOfWork.Enrollments.GetByCourseAsync(courseId);
        var target = enrollments.FirstOrDefault(e => e.StudentId == studentId);
        if (target != null)
        {
            await _unitOfWork.Enrollments.RemoveAsync(target.Id);
            await _unitOfWork.SaveChangesAsync();
        }
        TempData["CourseMessage"] = "Đã xoá sinh viên khỏi khoá học.";
        return RedirectToAction(nameof(Details), new { id = courseId });
    }
}

