using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class SubjectController : Controller
{
    [HttpPost]
    public IActionResult Subject(Subject subject)
    {
        // Simple implementation that redirects to SubjectList action
        return RedirectToAction("SubjectList", "Subject");
    }
    
    public IActionResult SubjectList()
    {
        return View();
    }
}

