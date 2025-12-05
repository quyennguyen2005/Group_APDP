using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public ProfileController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var userAccount = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (userAccount == null)
        {
            return NotFound();
        }

        // Get related student or instructor info if available
        Student? student = null;
        Instructor? instructor = null;
        Department? department = null;

        if (userAccount.StudentId.HasValue)
        {
            student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == userAccount.StudentId.Value);
            
            if (student?.DepartmentId.HasValue == true)
            {
                department = await _dbContext.Departments.FindAsync(student.DepartmentId.Value);
            }
        }

        if (userAccount.InstructorId.HasValue)
        {
            instructor = await _dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == userAccount.InstructorId.Value);
            
            if (instructor != null)
            {
                department = await _dbContext.Departments.FindAsync(instructor.DepartmentId);
            }
        }

        ViewBag.UserAccount = userAccount;
        ViewBag.Student = student;
        ViewBag.Instructor = instructor;
        ViewBag.Department = department;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var userAccount = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (userAccount == null)
        {
            return NotFound();
        }

        // Get related student or instructor info if available
        Student? student = null;
        Instructor? instructor = null;
        var departments = await _dbContext.Departments.ToListAsync();

        if (userAccount.StudentId.HasValue)
        {
            student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == userAccount.StudentId.Value);
        }

        if (userAccount.InstructorId.HasValue)
        {
            instructor = await _dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == userAccount.InstructorId.Value);
        }

        ViewBag.UserAccount = userAccount;
        ViewBag.Student = student;
        ViewBag.Instructor = instructor;
        ViewBag.Departments = departments;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? email, string? studentFullName, string? studentEmail, string? studentMajor, int? studentDepartmentId,
        string? instructorFullName, string? instructorEmail, string? instructorPhoneNumber, int? instructorDepartmentId)
    {
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var userAccount = await _dbContext.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == username);

        if (userAccount == null)
        {
            return NotFound();
        }

        try
        {
            // Update UserAccount email
            if (!string.IsNullOrEmpty(email))
            {
                userAccount.Email = email;
                _dbContext.UserAccounts.Update(userAccount);
            }

            // Update Student info if exists
            if (userAccount.StudentId.HasValue)
            {
                var student = await _dbContext.Students.FindAsync(userAccount.StudentId.Value);
                if (student != null)
                {
                    if (!string.IsNullOrEmpty(studentFullName))
                        student.FullName = studentFullName;
                    if (!string.IsNullOrEmpty(studentEmail))
                        student.Email = studentEmail;
                    if (!string.IsNullOrEmpty(studentMajor))
                        student.Major = studentMajor;
                    if (studentDepartmentId.HasValue)
                        student.DepartmentId = studentDepartmentId;

                    _dbContext.Students.Update(student);
                }
            }

            // Update Instructor info if exists
            if (userAccount.InstructorId.HasValue)
            {
                var instructor = await _dbContext.Instructors.FindAsync(userAccount.InstructorId.Value);
                if (instructor != null)
                {
                    if (!string.IsNullOrEmpty(instructorFullName))
                        instructor.FullName = instructorFullName;
                    if (!string.IsNullOrEmpty(instructorEmail))
                        instructor.Email = instructorEmail;
                    if (!string.IsNullOrEmpty(instructorPhoneNumber))
                        instructor.PhoneNumber = instructorPhoneNumber;
                    if (instructorDepartmentId.HasValue)
                        instructor.DepartmentId = instructorDepartmentId.Value;

                    _dbContext.Instructors.Update(instructor);
                }
            }

            await _dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Lỗi khi cập nhật: {ex.Message}";
            return RedirectToAction(nameof(Edit));
        }
    }
}

