using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Department
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Tên khoa")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Khối/Ngành")]
    public string Faculty { get; set; } = string.Empty;

    [Display(Name = "Văn phòng")]
    public string OfficeLocation { get; set; } = string.Empty;
}

