using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class UserAccount
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Role { get; set; } = "Student";

    public int? StudentId { get; set; }
    public int? InstructorId { get; set; }
}

