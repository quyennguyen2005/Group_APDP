using WebApp.Data;
using WebApp.Repositories;

namespace WebApp.UnitOfWork;

/// <summary>
/// Unit of Work pattern using Entity Framework Core.
/// </summary>
public class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public EfUnitOfWork(
        ApplicationDbContext context,
        IStudentRepository students,
        ICourseRepository courses,
        IEnrollmentRepository enrollments)
    {
        _context = context;
        Students = students;
        Courses = courses;
        Enrollments = enrollments;
    }

    public IStudentRepository Students { get; }
    public ICourseRepository Courses { get; }
    public IEnrollmentRepository Enrollments { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

