using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.Models;

namespace WebApp.Tests;

public class UnitTest1
{
    [Fact]
    public void Student_Add_ValidStudent_ReturnsRedirectToActionResult()
    {
        // Arrange
        var controller = new StudentController();
        
        // Act
        var result = controller.Student(new Student()) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("StudentList", result.ActionName);
        Assert.Equal("Student", result.ControllerName);
    }

    [Fact]
    public void Subject_Add_ValidSubject_ReturnsRedirectToActionResult()
    {
        // Arrange
        var controller = new SubjectController();
        
        // Act
        var result = controller.Subject(new Subject()) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("SubjectList", result.ActionName);
    }

    [Fact]
    public void Index_Post_ValidRegisterCourse_ReturnsRedirectToActionResult()
    {
        // Arrange
        var controller = new RegistercourseController();
        var registerCourse = new Registercourse();
        
        // Act
        var result = controller.Index(registerCourse) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Course", result.ActionName);
    }

    [Fact]
    public void Register_ValidUser_ReturnsRedirectToActionResult()
    {
        // Arrange
        var controller = new RegisterController();
        var user = new User
        {
            Id = "1001",
            UserName = "JohnDoe",
            Email = "johndoe@example.com",
            Password = "Password123"
        };
        
        // Act
        var result = controller.Register(user) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Login", result.ControllerName);
    }

    [Fact]
    public void Students_register_for_courses()
    {
        // Arrange
        var controller = new RegistercourseController();
        var registercouse = new Registercourse
        {
            Id = "BH01824",
            FirstName = "Hoang Quyen",
            LastName = "Nguyen",
            Address = "Ha Noi",
            Birthday = "17/09/2005",
            Gender = "Male",
            Email = "hoangquyennguyena6@gmail.com",
            PhoneNumber = "0914390644",
            SubjectName = "C#"
        };
        
        // Act
        var result = controller.Index(registercouse) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Course", result.ActionName);
    }

    [Fact]
    public void Login_RedirectsToAdminIndex_WhenAdminUser()
    {
        // Arrange
        var controller = new LoginController();
        var adminUser = new User { UserName = "Admin", Password = "123" };
        var users = new List<User> { adminUser };
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(users, options);
        File.WriteAllText("Data/users.json", jsonString);
        
        // Act
        var result = controller.Login(adminUser) as RedirectToActionResult;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Admin", result.ControllerName);
    }
}

