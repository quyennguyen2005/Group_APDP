using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Grade
{
    public int Id { get; set; }

    [Required]
    public int EnrollmentId { get; set; }

    [Range(0, 100)]
    public double AssignmentScore { get; set; }

    [Range(0, 100)]
    public double MidtermScore { get; set; }

    [Range(0, 100)]
    public double FinalScore { get; set; }

    [Display(Name = "Final Grade")]
    public string FinalGrade { get; set; } = "P";
}




