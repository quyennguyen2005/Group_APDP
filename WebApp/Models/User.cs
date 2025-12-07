using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "User Name")]
    public string UserName { get; set; } = string.Empty;
    
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

