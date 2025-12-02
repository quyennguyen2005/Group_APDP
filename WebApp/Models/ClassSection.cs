using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class ClassSection
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Khoá học")]
    public int CourseId { get; set; }

    [Required]
    [Display(Name = "Giảng viên")]
    public int InstructorId { get; set; }

    [Display(Name = "Học kỳ")]
    public string Semester { get; set; } = "1";

    [Display(Name = "Năm học")]
    public string AcademicYear { get; set; } = "2024-2025";

    [Display(Name = "Phòng học")]
    public string Room { get; set; } = "A1-101";

    [Display(Name = "Lịch học")]
    public string Schedule { get; set; } = "T2, T4, T6 - 08:00";
}




