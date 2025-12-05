using WebApp.Models;

namespace WebApp.Repositories;

public interface IEnrollmentRepository
{
    Task<IReadOnlyCollection<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Enrollment>> GetByCourseAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Enrollment>> GetByStudentAsync(int studentId, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetByIdAsync(int enrollmentId, CancellationToken cancellationToken = default);
    Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
    Task RemoveAsync(int enrollmentId, CancellationToken cancellationToken = default);
}

