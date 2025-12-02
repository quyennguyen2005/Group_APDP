using WebApp.Models;

namespace WebApp.Services.Classification;

public class GpaClassificationStrategy : IStudentClassificationStrategy
{
    public string Name => "GPA";

    public string GetClassification(Student student)
    {
        return student.Gpa switch
        {
            >= 3.7 => "Xuất sắc",
            >= 3.2 => "Giỏi",
            >= 2.5 => "Khá",
            >= 2.0 => "Trung bình",
            _ => "Yếu"
        };
    }
}

