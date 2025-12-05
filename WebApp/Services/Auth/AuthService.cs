using System.Security.Cryptography;
using System.Text;
using WebApp.Models;

namespace WebApp.Services.Auth;

public class AuthService
{
    private readonly Dictionary<string, UserAccount> _users = new(StringComparer.OrdinalIgnoreCase);

    public AuthService()
    {
        _users["admin"] = new UserAccount
        {
            Username = "admin",
            Password = HashPassword("password"),
            Email = "admin@example.com",
            Role = "Admin"
        };

        _users["teacher"] = new UserAccount
        {
            Username = "teacher",
            Password = HashPassword("password"),
            Email = "teacher@example.com",
            Role = "Instructor"
        };

        _users["student"] = new UserAccount
        {
            Username = "student",
            Password = HashPassword("password"),
            Email = "student@example.com",
            Role = "Student"
        };
    }

    public Task<(bool Success, string Message)> RegisterAsync(UserAccount account)
    {
        if (_users.ContainsKey(account.Username))
        {
            return Task.FromResult((false, "Tài khoản đã tồn tại."));
        }

        _users[account.Username] = new UserAccount
        {
            Username = account.Username,
            Password = HashPassword(account.Password),
            Email = account.Email,
            Role = string.IsNullOrWhiteSpace(account.Role) ? "Student" : account.Role
        };
        return Task.FromResult((true, "Đăng ký thành công!"));
    }

    public Task<(bool Success, string Message)> LoginAsync(UserAccount account)
    {
        if (_users.TryGetValue(account.Username, out var existing)
            && existing.Password == HashPassword(account.Password))
        {
            return Task.FromResult((true, "Đăng nhập thành công!"));
        }

        return Task.FromResult((false, "Sai tên đăng nhập hoặc mật khẩu."));
    }

    public IReadOnlyCollection<UserAccount> GetAllUsers()
    {
        return _users.Values
            .Select(u => new UserAccount
            {
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                StudentId = u.StudentId,
                InstructorId = u.InstructorId
            })
            .ToList()
            .AsReadOnly();
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}

