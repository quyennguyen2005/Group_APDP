using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Auth;
using WebApp.ViewModels;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly AuthService _authService;

    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewBag.LoginMessage = TempData["LoginMessage"] as string;
        ViewBag.VisitorName = TempData["VisitorName"] as string;
        return View(new LoginInput());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInput input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        var result = await _authService.LoginAsync(new UserAccount
        {
            Username = input.Username,
            Password = input.Password
        });

        if (result.Success)
        {
            HttpContext.Session.SetString("Username", input.Username);
            HttpContext.Session.SetString("IsAuthenticated", "true");
            TempData["WelcomeBack"] = $"Xin chào {input.Username}!";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.LoginMessage = result.Message;
        return View(input);
    }

    [HttpGet]
    public IActionResult Register()
    {
        ViewBag.RegisterMessage = TempData["RegisterMessage"] as string;
        return View(new RegisterInput());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterInput input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        var result = await _authService.RegisterAsync(new UserAccount
        {
            Username = input.Username,
            Password = input.Password,
            Email = input.Email,
            Role = input.Role
        });

        if (result.Success)
        {
            TempData["LoginMessage"] = result.Message + " Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        ViewBag.RegisterMessage = result.Message;
        return View(input);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        TempData["LoginMessage"] = "Đã đăng xuất thành công.";
        return RedirectToAction(nameof(Login));
    }
}

