using WebApp.Models;

namespace WebApp.Services.Classification;

public interface IStudentClassificationStrategy
{
    string Name { get; }
    string GetClassification(Student student);
}

