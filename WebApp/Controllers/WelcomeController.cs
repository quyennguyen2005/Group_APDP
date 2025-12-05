using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class WelcomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new WelcomeInput());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Start(WelcomeInput input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", input);
        }

        TempData["VisitorName"] = input.FullName;
        return RedirectToAction(nameof(AccountController.Login), "Account");
    }
}

