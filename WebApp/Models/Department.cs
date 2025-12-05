using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Department
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Department Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Faculty")]
    public string Faculty { get; set; } = string.Empty;

    [Display(Name = "Office Location")]
    public string OfficeLocation { get; set; } = string.Empty;
}




