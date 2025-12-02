using WebApp.Repositories;

namespace WebApp.UnitOfWork;

/// <summary>
/// Unit of Work pattern: ensures we have a single coordination point for repositories.
/// </summary>
public class InMemoryUnitOfWork : IUnitOfWork
{
    public InMemoryUnitOfWork(
        IStudentRepository students,
        ICourseRepository courses,
        IEnrollmentRepository enrollments)
    {
        Students = students;
        Courses = courses;
        Enrollments = enrollments;
    }

    public IStudentRepository Students { get; }
    public ICourseRepository Courses { get; }
    public IEnrollmentRepository Enrollments { get; }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In-memory store commits immediately, but we keep the abstraction available.
        return Task.CompletedTask;
    }
}

