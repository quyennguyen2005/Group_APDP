using WebApp.Models;

namespace WebApp.Services.Classification;

public enum ClassificationMode
{
    Gpa,
    Credit
}

public class StudentClassificationService
{
    private readonly IReadOnlyDictionary<ClassificationMode, IStudentClassificationStrategy> _strategies;

    public StudentClassificationService(IEnumerable<IStudentClassificationStrategy> strategies)
    {
        _strategies = strategies switch
        {
            IReadOnlyDictionary<ClassificationMode, IStudentClassificationStrategy> typed => typed,
            _ => strategies.ToDictionary(MapMode, strategy => strategy)
        };
    }

    public string Describe(Student student, ClassificationMode mode)
    {
        return _strategies.TryGetValue(mode, out var strategy)
            ? strategy.GetClassification(student)
            : "Không xác định";
    }

    private static ClassificationMode MapMode(IStudentClassificationStrategy strategy)
    {
        return strategy switch
        {
            GpaClassificationStrategy => ClassificationMode.Gpa,
            CreditMomentumStrategy => ClassificationMode.Credit,
            _ => ClassificationMode.Gpa
        };
    }
}

