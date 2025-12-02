using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Mã khoá học")]
    public string CourseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [Display(Name = "Tên khoá học")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Range(1, 10)]
    [Display(Name = "Số tín chỉ")]
    public int Credits { get; set; } = 3;

    [Display(Name = "Giảng viên phụ trách")]
    public string Instructor { get; set; } = string.Empty;

    [Display(Name = "Học kỳ")]
    public string Semester { get; set; } = "2024-1";

    [Display(Name = "Ngày bắt đầu")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [Display(Name = "Ngày kết thúc")]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddMonths(3);

    [Range(1, 200)]
    [Display(Name = "Sĩ số tối đa")]
    public int MaxStudents { get; set; } = 50;

    [Display(Name = "Khoa phụ trách")]
    public int? DepartmentId { get; set; }
}

