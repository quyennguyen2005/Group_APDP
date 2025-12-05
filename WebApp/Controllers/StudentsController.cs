using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Classification;
using WebApp.UnitOfWork;

namespace WebApp.Controllers;

public class StudentsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly StudentClassificationService _classificationService;

    public StudentsController(IUnitOfWork unitOfWork, StudentClassificationService classificationService)
    {
        _unitOfWork = unitOfWork;
        _classificationService = classificationService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _unitOfWork.Students.GetAllAsync();
        var items = students
            .Select(student => new StudentListItemViewModel
            {
                Student = student,
                GpaRank = _classificationService.Describe(student, ClassificationMode.Gpa),
                CreditRank = _classificationService.Describe(student, ClassificationMode.Credit)
            })
            .OrderByDescending(i => i.Student.Gpa)
            .ToList();

        return View(items);
    }

    public IActionResult Create()
    {
        return View(new Student());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        student.EnrollmentDate = DateTime.UtcNow;
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(student);
        }

        await _unitOfWork.Students.UpdateAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        var vm = new StudentDetailsViewModel
        {
            Student = student,
            GpaRank = _classificationService.Describe(student, ClassificationMode.Gpa),
            CreditRank = _classificationService.Describe(student, ClassificationMode.Credit)
        };

        return View(vm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _unitOfWork.Students.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

public class StudentListItemViewModel
{
    public required Student Student { get; set; }
    public required string GpaRank { get; set; }
    public required string CreditRank { get; set; }
}

public class StudentDetailsViewModel : StudentListItemViewModel
{
}

