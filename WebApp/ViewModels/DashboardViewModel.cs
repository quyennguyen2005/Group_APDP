using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels;

public class DashboardViewModel
{
    public int TotalStudents { get; set; }
    public double AverageGpa { get; set; }
    public int TotalCredits { get; set; }
    public int ActiveCourses { get; set; }
    public int PendingAssignments { get; set; }
    public int UnreadMessages { get; set; }
    public IReadOnlyCollection<Student> TopStudents { get; set; } = Array.Empty<Student>();
    public IReadOnlyCollection<ActivityLog> RecentActivities { get; set; } = Array.Empty<ActivityLog>();
    public IReadOnlyCollection<DepartmentSummary> Departments { get; set; } = Array.Empty<DepartmentSummary>();
    public IReadOnlyCollection<CourseOverview> Courses { get; set; } = Array.Empty<CourseOverview>();
    public IReadOnlyCollection<ClassOverview> Classes { get; set; } = Array.Empty<ClassOverview>();
    public IReadOnlyCollection<EnrollmentStatusSummary> EnrollmentStatuses { get; set; } = Array.Empty<EnrollmentStatusSummary>();
    public IReadOnlyCollection<GradeOverview> Grades { get; set; } = Array.Empty<GradeOverview>();
    public IReadOnlyCollection<AccountSummary> Accounts { get; set; } = Array.Empty<AccountSummary>();
}

public class ActivityLog
{
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class DepartmentSummary
{
    public string Name { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public int CourseCount { get; set; }
    public string Office { get; set; } = string.Empty;
}

public class CourseOverview
{
    public string CourseCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string Department { get; set; } = string.Empty;
}

public class ClassOverview
{
    public string Name { get; set; } = string.Empty;
    public string Semester { get; set; } = string.Empty;
    public string AcademicYear { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
}

public class EnrollmentStatusSummary
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class GradeOverview
{
    public string Grade { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class AccountSummary
{
    public string Role { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class LoginInput
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterInput : LoginInput
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Role")]
    public string Role { get; set; } = "Student";
}

public class WelcomeInput
{
    [Required]
    [Display(Name = "Your Name")]
    public string FullName { get; set; } = string.Empty;
}

