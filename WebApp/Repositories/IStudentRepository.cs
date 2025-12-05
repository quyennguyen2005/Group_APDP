using WebApp.Models;

namespace WebApp.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default);
    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
    Task RemoveAsync(int id, CancellationToken cancellationToken = default);
}

