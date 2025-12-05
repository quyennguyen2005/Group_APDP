using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class ClassSection
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Required]
    [Display(Name = "Instructor")]
    public int InstructorId { get; set; }

    [Display(Name = "Semester")]
    public string Semester { get; set; } = "1";

    [Display(Name = "Academic Year")]
    public string AcademicYear { get; set; } = "2024-2025";

    [Display(Name = "Room")]
    public string Room { get; set; } = "A1-101";

    [Display(Name = "Schedule")]
    public string Schedule { get; set; } = "Mon, Wed, Fri - 08:00";
}




