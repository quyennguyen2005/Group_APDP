using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly InMemoryDataContext _context;

    public EnrollmentRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Enrollments);
    }

    public Task<IReadOnlyCollection<Enrollment>> GetByCourseAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var items = _context.Enrollments
            .Where(e => e.CourseId == courseId)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyCollection<Enrollment>)items);
    }

    public Task<IReadOnlyCollection<Enrollment>> GetByStudentAsync(int studentId, CancellationToken cancellationToken = default)
    {
        var items = _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToList()
            .AsReadOnly();
        return Task.FromResult((IReadOnlyCollection<Enrollment>)items);
    }

    public Task<Enrollment?> GetByIdAsync(int enrollmentId, CancellationToken cancellationToken = default)
    {
        var enrollment = _context.Enrollments.FirstOrDefault(e => e.Id == enrollmentId);
        return Task.FromResult(enrollment);
    }

    public Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        var created = _context.AddEnrollment(enrollment);
        return Task.FromResult(created);
    }

    public Task RemoveAsync(int enrollmentId, CancellationToken cancellationToken = default)
    {
        _context.RemoveEnrollment(enrollmentId);
        return Task.CompletedTask;
    }
}

