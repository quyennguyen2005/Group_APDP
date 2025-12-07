using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class StudentController : Controller
{
    [HttpPost]
    public IActionResult Student(Student student)
    {
        // Simple implementation that redirects to StudentList action
        return RedirectToAction("StudentList", "Student");
    }
    
    public IActionResult StudentList()
    {
        return View();
    }
}

