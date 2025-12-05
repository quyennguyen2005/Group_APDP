using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Student Code")]
    [StringLength(30)]
    public string StudentCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Major")]
    [StringLength(100)]
    public string Major { get; set; } = "General";

    [Range(0, 4)]
    [Display(Name = "GPA")]
    public double Gpa { get; set; }

    [Range(0, 200)]
    [Display(Name = "Total Credits")]
    public int TotalCredits { get; set; }

    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }
}

