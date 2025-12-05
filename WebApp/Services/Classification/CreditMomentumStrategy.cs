using WebApp.Models;

namespace WebApp.Services.Classification;

public class CreditMomentumStrategy : IStudentClassificationStrategy
{
    public string Name => "Tín chỉ";

    public string GetClassification(Student student)
    {
        if (student.TotalCredits >= 100)
        {
            return "Hoàn thành > 100 TC";
        }

        if (student.TotalCredits >= 80)
        {
            return "Tiến độ tốt";
        }

        if (student.TotalCredits >= 60)
        {
            return "Cần tăng tốc";
        }

        return "Mới nhập học";
    }
}

