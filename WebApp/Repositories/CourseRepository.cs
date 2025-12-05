using WebApp.Data;
using WebApp.Models;

namespace WebApp.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly InMemoryDataContext _context;

    public CourseRepository(InMemoryDataContext context)
    {
        _context = context;
    }

    public Task<IReadOnlyCollection<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_context.Courses);
    }

    public Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(course);
    }

    public Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        var created = _context.AddCourse(course);
        return Task.FromResult(created);
    }

    public Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.UpdateCourse(course);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        _context.RemoveCourse(id);
        return Task.CompletedTask;
    }
}

