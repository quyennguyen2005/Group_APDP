using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Auth;
using WebApp.UnitOfWork;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _dbContext;
        private readonly AuthService _authService;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ApplicationDbContext dbContext, AuthService authService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _unitOfWork.Students.GetAllAsync();
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();
            var departments = await _dbContext.Departments.ToListAsync();
            var classes = await _dbContext.ClassSections.ToListAsync();
            var instructors = await _dbContext.Instructors.ToListAsync();
            var grades = await _dbContext.Grades.ToListAsync();
            var accounts = _authService.GetAllUsers();
            
            var username = HttpContext.Session.GetString("Username");
            var activities = new List<ActivityLog>
            {
                new ActivityLog { Action = "login", Timestamp = DateTime.Now.AddHours(-2) },
                new ActivityLog { Action = "login", Timestamp = DateTime.Now.AddDays(-1) },
                new ActivityLog { Action = "login", Timestamp = DateTime.Now.AddDays(-2) },
                new ActivityLog { Action = "logout", Timestamp = DateTime.Now.AddDays(-3) }
            };
            
            var departmentSummaries = departments
                .Select(d => new DepartmentSummary
                {
                    Name = d.Name,
                    Faculty = d.Faculty,
                    Office = d.OfficeLocation,
                    StudentCount = students.Count(s => (s.DepartmentId.HasValue && s.DepartmentId == d.Id) || s.Major.Equals(d.Name, StringComparison.OrdinalIgnoreCase)),
                    CourseCount = courses.Count(c => c.DepartmentId == d.Id)
                })
                .ToList();

            var courseSummaries = courses
                .Select(c =>
                {
                    var departmentName = departments.FirstOrDefault(d => d.Id == c.DepartmentId)?.Name ?? "Other";
                    return new CourseOverview
                    {
                        CourseCode = c.CourseCode,
                        Title = c.Title,
                        Credits = c.Credits,
                        Department = departmentName
                    };
                })
                .ToList();

            var classSummaries = classes
                .Select(cls =>
                {
                    var course = courses.FirstOrDefault(c => c.Id == cls.CourseId);
                    var instructor = instructors.FirstOrDefault(i => i.Id == cls.InstructorId);
                    return new ClassOverview
                    {
                        Name = course?.Title ?? $"Class #{cls.Id}",
                        Semester = cls.Semester,
                        AcademicYear = cls.AcademicYear,
                        Instructor = instructor?.FullName ?? "Not assigned",
                        Schedule = $"{cls.Room} • {cls.Schedule}"
                    };
                })
                .ToList();

            var enrollmentStatus = enrollments
                .GroupBy(e => e.Status)
                .Select(g => new EnrollmentStatusSummary
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            var gradeOverview = grades
                .GroupBy(g => g.FinalGrade)
                .Select(g => new GradeOverview
                {
                    Grade = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var accountOverview = accounts
                .GroupBy(a => a.Role)
                .Select(g => new AccountSummary
                {
                    Role = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var vm = new DashboardViewModel
            {
                TotalStudents = students.Count,
                AverageGpa = students.Any() ? Math.Round(students.Average(s => s.Gpa), 2) : 0,
                TotalCredits = students.Sum(s => s.TotalCredits),
                ActiveCourses = courses.Count,
                PendingAssignments = grades.Count(g => g.FinalScore < 70),
                UnreadMessages = 0,
                TopStudents = students.OrderByDescending(s => s.Gpa).Take(3).ToList(),
                RecentActivities = activities,
                Departments = departmentSummaries,
                Courses = courseSummaries,
                Classes = classSummaries,
                EnrollmentStatuses = enrollmentStatus,
                Grades = gradeOverview,
                Accounts = accountOverview
            };
            ViewBag.WelcomeBack = TempData["WelcomeBack"] as string;
            ViewBag.Username = username;
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
