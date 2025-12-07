using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class RegisterController : Controller
{
    [HttpPost]
    public IActionResult Register(User user)
    {
        // Simple implementation that redirects to Index action in Login controller
        return RedirectToAction("Index", "Login");
    }
}

