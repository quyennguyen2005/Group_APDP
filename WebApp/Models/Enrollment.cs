using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Required]
    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Display(Name = "Status")]
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
}

public enum EnrollmentStatus
{
    Active,
    Completed,
    Dropped
}

