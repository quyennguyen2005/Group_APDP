using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories;

/// <summary>
/// Repository pattern: centralize data access logic for students.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Students.ToListAsync(cancellationToken);
    }

    public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Students.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync(cancellationToken);
        return student;
    }

    public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        var student = await _context.Students.FindAsync(new object[] { id }, cancellationToken);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

