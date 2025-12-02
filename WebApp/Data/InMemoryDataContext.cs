using WebApp.Models;

namespace WebApp.Data;

/// <summary>
/// Acts as a lightweight data store so we can focus on patterns without a database.
/// </summary>
public class InMemoryDataContext
{
    private readonly List<Student> _students = new();
    private readonly List<Course> _courses = new();
    private readonly List<Enrollment> _enrollments = new();
    private readonly List<Department> _departments = new();
    private readonly List<Instructor> _instructors = new();
    private readonly List<ClassSection> _classes = new();
    private readonly List<Grade> _grades = new();
    private int _nextStudentId = 1;
    private int _nextCourseId = 1;
    private int _nextEnrollmentId = 1;
    private int _nextDepartmentId = 1;
    private int _nextInstructorId = 1;
    private int _nextClassId = 1;
    private int _nextGradeId = 1;

    public InMemoryDataContext()
    {
        Seed();
    }

    public IReadOnlyCollection<Student> Students => _students.AsReadOnly();
    public IReadOnlyCollection<Course> Courses => _courses.AsReadOnly();
    public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();
    public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();
    public IReadOnlyCollection<Instructor> Instructors => _instructors.AsReadOnly();
    public IReadOnlyCollection<ClassSection> Classes => _classes.AsReadOnly();
    public IReadOnlyCollection<Grade> Grades => _grades.AsReadOnly();

    public Student AddStudent(Student student)
    {
        student.Id = _nextStudentId++;
        _students.Add(student);
        return student;
    }

    public void UpdateStudent(Student student)
    {
        var index = _students.FindIndex(s => s.Id == student.Id);
        if (index >= 0)
        {
            _students[index] = student;
        }
    }

    public void RemoveStudent(int id)
    {
        var target = _students.FirstOrDefault(s => s.Id == id);
        if (target != null)
        {
            _students.Remove(target);
        }
    }

    public Course AddCourse(Course course)
    {
        course.Id = _nextCourseId++;
        _courses.Add(course);
        return course;
    }

    public void UpdateCourse(Course course)
    {
        var index = _courses.FindIndex(c => c.Id == course.Id);
        if (index >= 0)
        {
            _courses[index] = course;
        }
    }

    public void RemoveCourse(int id)
    {
        var target = _courses.FirstOrDefault(c => c.Id == id);
        if (target != null)
        {
            _courses.Remove(target);
            _enrollments.RemoveAll(e => e.CourseId == id);
        }
    }

    public Enrollment AddEnrollment(Enrollment enrollment)
    {
        var exists = _enrollments.Any(e => e.StudentId == enrollment.StudentId && e.CourseId == enrollment.CourseId);
        if (exists)
        {
            return enrollment;
        }

        enrollment.Id = _nextEnrollmentId++;
        _enrollments.Add(enrollment);
        return enrollment;
    }

    public void RemoveEnrollment(int enrollmentId)
    {
        var target = _enrollments.FirstOrDefault(e => e.Id == enrollmentId);
        if (target != null)
        {
            _enrollments.Remove(target);
        }
    }

    private void Seed()
    {
        if (_students.Any())
        {
            return;
        }

        var now = DateTime.UtcNow;

        var deptCs = AddDepartment(new Department
        {
            Name = "Computer Science",
            Faculty = "CNTT",
            OfficeLocation = "Tòa A1"
        });
        var deptIs = AddDepartment(new Department
        {
            Name = "Information Systems",
            Faculty = "CNTT",
            OfficeLocation = "Tòa A2"
        });

        var s1 = AddStudent(new Student
        {
            StudentCode = "STU001",
            FullName = "Nguyen Van A",
            Email = "a.nguyen@example.com",
            Major = deptCs.Name,
            DepartmentId = deptCs.Id,
            Gpa = 3.6,
            TotalCredits = 96,
            EnrollmentDate = now.AddYears(-3)
        });
        var s2 = AddStudent(new Student
        {
            StudentCode = "STU002",
            FullName = "Tran Thi B",
            Email = "b.tran@example.com",
            Major = deptIs.Name,
            DepartmentId = deptIs.Id,
            Gpa = 3.2,
            TotalCredits = 80,
            EnrollmentDate = now.AddYears(-2)
        });
        var s3 = AddStudent(new Student
        {
            StudentCode = "STU003",
            FullName = "Le Hoang C",
            Email = "c.le@example.com",
            Major = deptCs.Name,
            DepartmentId = deptCs.Id,
            Gpa = 3.9,
            TotalCredits = 110,
            EnrollmentDate = now.AddYears(-4)
        });

        var instructor1 = AddInstructor(new Instructor
        {
            FullName = "ThS. Nguyen Minh",
            Email = "minh.nguyen@btec.edu.vn",
            PhoneNumber = "0901122334",
            DepartmentId = deptCs.Id
        });
        var instructor2 = AddInstructor(new Instructor
        {
            FullName = "TS. Tran Lan",
            Email = "lan.tran@btec.edu.vn",
            PhoneNumber = "0905566778",
            DepartmentId = deptIs.Id
        });

        var course1 = AddCourse(new Course
        {
            CourseCode = "CSI101",
            Title = "Nhập môn lập trình",
            Description = "Các khái niệm nền tảng về lập trình và cấu trúc dữ liệu.",
            Credits = 3,
            Instructor = instructor1.FullName,
            Semester = "2024-1",
            StartDate = now.AddDays(-30),
            EndDate = now.AddDays(60),
            MaxStudents = 40,
            DepartmentId = deptCs.Id
        });
        var course2 = AddCourse(new Course
        {
            CourseCode = "DBI201",
            Title = "Cơ sở dữ liệu",
            Description = "Thiết kế và truy vấn cơ sở dữ liệu quan hệ.",
            Credits = 4,
            Instructor = instructor2.FullName,
            Semester = "2024-1",
            StartDate = now.AddDays(-15),
            EndDate = now.AddDays(75),
            MaxStudents = 35,
            DepartmentId = deptIs.Id
        });

        var class1 = AddClassSection(new ClassSection
        {
            CourseId = course1.Id,
            InstructorId = instructor1.Id,
            Semester = "Fall",
            AcademicYear = "2024-2025",
            Room = "Lab 101",
            Schedule = "T2, T4 - 08:00"
        });
        var class2 = AddClassSection(new ClassSection
        {
            CourseId = course2.Id,
            InstructorId = instructor2.Id,
            Semester = "Fall",
            AcademicYear = "2024-2025",
            Room = "Room 205",
            Schedule = "T3, T5 - 13:30"
        });

        var e1 = AddEnrollment(new Enrollment { StudentId = s1.Id, CourseId = course1.Id });
        var e2 = AddEnrollment(new Enrollment { StudentId = s2.Id, CourseId = course1.Id });
        var e3 = AddEnrollment(new Enrollment { StudentId = s3.Id, CourseId = course2.Id });

        AddGrade(new Grade { EnrollmentId = e1.Id, AssignmentScore = 85, MidtermScore = 80, FinalScore = 90, FinalGrade = "A" });
        AddGrade(new Grade { EnrollmentId = e2.Id, AssignmentScore = 78, MidtermScore = 75, FinalScore = 82, FinalGrade = "B" });
        AddGrade(new Grade { EnrollmentId = e3.Id, AssignmentScore = 92, MidtermScore = 88, FinalScore = 91, FinalGrade = "A" });
    }

    private Department AddDepartment(Department department)
    {
        department.Id = _nextDepartmentId++;
        _departments.Add(department);
        return department;
    }

    private Instructor AddInstructor(Instructor instructor)
    {
        instructor.Id = _nextInstructorId++;
        _instructors.Add(instructor);
        return instructor;
    }

    private ClassSection AddClassSection(ClassSection section)
    {
        section.Id = _nextClassId++;
        _classes.Add(section);
        return section;
    }

    private Grade AddGrade(Grade grade)
    {
        grade.Id = _nextGradeId++;
        _grades.Add(grade);
        return grade;
    }
}

