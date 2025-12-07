using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class RegistercourseController : Controller
{
    [HttpPost]
    public IActionResult Index(Registercourse registerCourse)
    {
        // Simple implementation that redirects to Course action
        return RedirectToAction("Course");
    }
    
    public IActionResult Course()
    {
        return View();
    }
}

