using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Registercourse
{
    public string Id { get; set; } = string.Empty;
    
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string Birthday { get; set; } = string.Empty;
    
    public string Gender { get; set; } = string.Empty;
    
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Display(Name = "Subject Name")]
    public string SubjectName { get; set; } = string.Empty;
}

