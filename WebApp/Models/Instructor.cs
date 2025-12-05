using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Instructor
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Department")]
    public int DepartmentId { get; set; }
}




