using WebApp.Repositories;

namespace WebApp.UnitOfWork;

public interface IUnitOfWork
{
    IStudentRepository Students { get; }
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

