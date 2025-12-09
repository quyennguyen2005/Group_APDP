using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers;

public class LoginController : Controller
{
    [HttpPost]
    public IActionResult Login(User user)
    {
        // Simple implementation that redirects to Index action in Admin controller
        // For admin user, redirect to Admin Index
        if (user.UserName == "Admin" && user.Password == "123")
        {
            return RedirectToAction("Index", "Admin");
        }
        else
        {
            // For other users, redirect to Home Index
            TempData["ErrorMessage"] = "Invalid username or password.";
        }
            return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Index()
    {
        return View();
    }
}

