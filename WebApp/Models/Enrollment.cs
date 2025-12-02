using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Sinh viên")]
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Khoá học")]
    public int CourseId { get; set; }

    [Display(Name = "Trạng thái")]
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
}

public enum EnrollmentStatus
{
    Active,
    Completed,
    Dropped
}

