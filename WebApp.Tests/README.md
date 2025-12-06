# WebApp.Tests

Project test automation cho WebApp application.

## Cấu trúc Test

### 1. Unit Tests
- **AuthServiceTests.cs**: Test các chức năng authentication
  - Login với credentials hợp lệ/không hợp lệ
  - Register user mới
  - Get user by username
  - Password hashing và security

### 2. Repository Tests  
- **CourseRepositoryTests.cs**: Test các CRUD operations cho Course repository
  - Add, Get, Update, Delete courses
  - Sử dụng InMemory database để test nhanh

### 3. Integration Tests
- **AccountControllerTests.cs**: Test các action của AccountController
  - Login page
  - Register page
  - Form validation
  
- **CoursesControllerTests.cs**: Test các action của CoursesController
  - Index, Details, Create, Edit, Delete
  - Student enrollment
  - Authorization checks

## Chạy Tests

### Chạy tất cả tests:
```bash
dotnet test
```

### Chạy tests trong một class cụ thể:
```bash
dotnet test --filter "FullyQualifiedName~AuthServiceTests"
```

### Chạy một test cụ thể:
```bash
dotnet test --filter "FullyQualifiedName~AuthServiceTests.LoginAsync_WithValidCredentials_ReturnsSuccess"
```

### Chạy với output chi tiết:
```bash
dotnet test --logger "console;verbosity=detailed"
```

## Dependencies

- **xUnit**: Testing framework
- **Moq**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing cho ASP.NET Core
- **Microsoft.EntityFrameworkCore.InMemory**: InMemory database cho testing

## Ghi chú

- Tất cả tests sử dụng InMemory database để đảm bảo test nhanh và độc lập
- Integration tests sử dụng WebApplicationFactory để test toàn bộ HTTP pipeline
- Mỗi test method được thiết kế để độc lập và có thể chạy riêng lẻ
