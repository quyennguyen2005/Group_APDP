using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Subject
{
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Subject Name")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Description")]
    public string? Description { get; set; }
}

