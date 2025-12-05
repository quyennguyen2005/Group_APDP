using WebApp.Models;

namespace WebApp.ViewModels;

public class CourseListItemViewModel
{
    public required Course Course { get; set; }
    public int EnrolledCount { get; set; }
}

public class CourseDetailsViewModel
{
    public required Course Course { get; set; }
    public IReadOnlyCollection<Student> EnrolledStudents { get; set; } = Array.Empty<Student>();
    public IReadOnlyCollection<Student> AvailableStudents { get; set; } = Array.Empty<Student>();
    public int? SelectedStudentId { get; set; }
}

