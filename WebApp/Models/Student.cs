using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Mã sinh viên")]
    [StringLength(30)]
    public string StudentCode { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Họ tên")]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Chuyên ngành")]
    [StringLength(100)]
    public string Major { get; set; } = "General";

    [Range(0, 4)]
    [Display(Name = "GPA")]
    public double Gpa { get; set; }

    [Range(0, 200)]
    [Display(Name = "Tổng tín chỉ")]
    public int TotalCredits { get; set; }

    [Display(Name = "Ngày nhập học")]
    public DateTime EnrollmentDate { get; set; }

    [Display(Name = "Khoa")]
    public int? DepartmentId { get; set; }
}

