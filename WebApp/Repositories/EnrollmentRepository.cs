using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _context;

    public EnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Enrollment>> GetByCourseAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .Where(e => e.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Enrollment>> GetByStudentAsync(int studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetByIdAsync(int enrollmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments.FindAsync(new object[] { enrollmentId }, cancellationToken);
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync(cancellationToken);
        return enrollment;
    }

    public async Task RemoveAsync(int enrollmentId, CancellationToken cancellationToken = default)
    {
        var enrollment = await _context.Enrollments.FindAsync(new object[] { enrollmentId }, cancellationToken);
        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

