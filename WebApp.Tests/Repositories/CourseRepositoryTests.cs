using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Tests.Repositories;

public class CourseRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly CourseRepository _repository;

    public CourseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new CourseRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCourse()
    {
        // Arrange
        var course = new Course
        {
            CourseCode = "TEST101",
            Title = "Test Course",
            Credits = 3,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 30
        };

        // Act
        var result = await _repository.AddAsync(course);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("TEST101", result.CourseCode);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsCourse()
    {
        // Arrange
        var course = new Course
        {
            CourseCode = "TEST101",
            Title = "Test Course",
            Credits = 3,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 30
        };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        Assert.Equal("TEST101", result.CourseCode);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(99999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCourses()
    {
        // Arrange
        _context.Courses.Add(new Course
        {
            CourseCode = "TEST101",
            Title = "Test Course 1",
            Credits = 3,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 30
        });
        _context.Courses.Add(new Course
        {
            CourseCode = "TEST102",
            Title = "Test Course 2",
            Credits = 4,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 25
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        Assert.Contains(result, c => c.CourseCode == "TEST101");
        Assert.Contains(result, c => c.CourseCode == "TEST102");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCourse()
    {
        // Arrange
        var course = new Course
        {
            CourseCode = "TEST101",
            Title = "Test Course",
            Credits = 3,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 30
        };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        course.Title = "Updated Course Title";
        await _repository.UpdateAsync(course);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _repository.GetByIdAsync(course.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Course Title", updated.Title);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveCourse()
    {
        // Arrange
        var course = new Course
        {
            CourseCode = "TEST101",
            Title = "Test Course",
            Credits = 3,
            Instructor = "Test Instructor",
            Semester = "2024-1",
            StartDate = DateTime.UtcNow.Date,
            EndDate = DateTime.UtcNow.Date.AddMonths(3),
            MaxStudents = 30
        };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        var courseId = course.Id;

        // Act
        await _repository.RemoveAsync(courseId);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(courseId);
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
