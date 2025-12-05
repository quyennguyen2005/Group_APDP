using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories;

/// <summary>
/// Repository pattern: centralize data access logic for students.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly InMemoryDataContext _context;

    public StudentRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Students);
    }

    public Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = _context.Students.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(student);
    }

    public Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        var created = _context.AddStudent(student);
        return Task.FromResult(created);
    }

    public Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        _context.UpdateStudent(student);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        _context.RemoveStudent(id);
        return Task.CompletedTask;
    }
}

