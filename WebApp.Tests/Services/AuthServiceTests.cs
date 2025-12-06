using WebApp.Models;
using WebApp.Services.Auth;

namespace WebApp.Tests.Services;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService();
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "admin",
            Password = "password"
        };

        // Act
        var result = await _authService.LoginAsync(account);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Login successful!", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidUsername_ReturnsFailure()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "invaliduser",
            Password = "password"
        };

        // Act
        var result = await _authService.LoginAsync(account);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid username or password.", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsFailure()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "admin",
            Password = "wrongpassword"
        };

        // Act
        var result = await _authService.LoginAsync(account);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid username or password.", result.Message);
    }

    [Fact]
    public async Task LoginAsync_WithCaseInsensitiveUsername_ReturnsSuccess()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "ADMIN", // uppercase
            Password = "password"
        };

        // Act
        var result = await _authService.LoginAsync(account);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task RegisterAsync_WithNewUser_ReturnsSuccess()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "newuser",
            Password = "password123",
            Email = "newuser@example.com",
            Role = "Student"
        };

        // Act
        var result = await _authService.RegisterAsync(account);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Registration successful!", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingUsername_ReturnsFailure()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "admin", // already exists
            Password = "password123",
            Email = "admin2@example.com",
            Role = "Student"
        };

        // Act
        var result = await _authService.RegisterAsync(account);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Account already exists.", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_WithEmptyRole_SetsDefaultRoleToStudent()
    {
        // Arrange
        var account = new UserAccount
        {
            Username = "defaultroleuser",
            Password = "password123",
            Email = "defaultrole@example.com",
            Role = "" // empty role
        };

        // Act
        var result = await _authService.RegisterAsync(account);

        // Assert
        Assert.True(result.Success);
        
        var user = await _authService.GetUserByUsernameAsync("defaultroleuser");
        Assert.NotNull(user);
        Assert.Equal("Student", user.Role);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithExistingUser_ReturnsUser()
    {
        // Arrange
        var username = "admin";

        // Act
        var user = await _authService.GetUserByUsernameAsync(username);

        // Assert
        Assert.NotNull(user);
        Assert.Equal("admin", user.Username);
        Assert.Equal("admin@example.com", user.Email);
        Assert.Equal("Admin", user.Role);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithNonExistingUser_ReturnsNull()
    {
        // Arrange
        var username = "nonexistentuser";

        // Act
        var user = await _authService.GetUserByUsernameAsync(username);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ShouldNotReturnPassword()
    {
        // Arrange
        var username = "admin";

        // Act
        var user = await _authService.GetUserByUsernameAsync(username);

        // Assert
        Assert.NotNull(user);
        Assert.Empty(user.Password); // Password should not be exposed
    }

    [Fact]
    public void GetAllUsers_ReturnsAllUsers()
    {
        // Act
        var users = _authService.GetAllUsers();

        // Assert
        Assert.NotNull(users);
        Assert.True(users.Count >= 3); // At least admin, teacher, student
        Assert.Contains(users, u => u.Username == "admin");
        Assert.Contains(users, u => u.Username == "teacher");
        Assert.Contains(users, u => u.Username == "student");
    }

    [Fact]
    public void GetAllUsers_ShouldNotReturnPasswords()
    {
        // Act
        var users = _authService.GetAllUsers();

        // Assert
        Assert.All(users, user => Assert.Empty(user.Password));
    }

    [Fact]
    public async Task RegisterAsync_ThenLoginAsync_ShouldWork()
    {
        // Arrange
        var username = "testuser" + Guid.NewGuid().ToString()[..8];
        var password = "testpass123";
        var account = new UserAccount
        {
            Username = username,
            Password = password,
            Email = "test@example.com",
            Role = "Student"
        };

        // Act - Register
        var registerResult = await _authService.RegisterAsync(account);
        Assert.True(registerResult.Success);

        // Act - Login
        var loginResult = await _authService.LoginAsync(new UserAccount
        {
            Username = username,
            Password = password
        });

        // Assert
        Assert.True(loginResult.Success);
    }
}
