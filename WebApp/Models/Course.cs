using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Course Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Range(1, 10)]
    [Display(Name = "Credits")]
    public int Credits { get; set; } = 3;

    [Display(Name = "Instructor")]
    public string Instructor { get; set; } = string.Empty;

    [Display(Name = "Semester")]
    public string Semester { get; set; } = "2024-1";

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddMonths(3);

    [Range(1, 200)]
    [Display(Name = "Max Students")]
    public int MaxStudents { get; set; } = 50;

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }
}

