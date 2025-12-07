# Hướng Dẫn Sử Dụng XUnit trong WebApp.Tests

## Giới Thiệu về XUnit

XUnit là một testing framework phổ biến cho .NET, được sử dụng để viết và chạy automation tests trong project này.

## Cấu Trúc XUnit Tests

### 1. Test Class

Mỗi test class phải là `public`:

```csharp
public class AuthServiceTests
{
    // Test methods here
}
```

### 2. Test Method

Test method được đánh dấu bằng attribute `[Fact]`:

```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
{
    // Arrange: Chuẩn bị dữ liệu
    var account = new UserAccount { Username = "admin", Password = "password" };
    
    // Act: Thực hiện hành động cần test
    var result = await _authService.LoginAsync(account);
    
    // Assert: Kiểm tra kết quả
    Assert.True(result.Success);
}
```

### 3. Theory Tests (Tests với nhiều input)

Sử dụng `[Theory]` và `[InlineData]` để test với nhiều dữ liệu khác nhau:

```csharp
[Theory]
[InlineData("admin", "password", true)]
[InlineData("admin", "wrong", false)]
[InlineData("invalid", "password", false)]
public async Task LoginAsync_WithVariousInputs_ReturnsExpectedResult(
    string username, string password, bool expectedSuccess)
{
    var account = new UserAccount { Username = username, Password = password };
    var result = await _authService.LoginAsync(account);
    Assert.Equal(expectedSuccess, result.Success);
}
```

## Assertions trong XUnit

### Assert.True / Assert.False

```csharp
Assert.True(result.Success);  // Kiểm tra giá trị là true
Assert.False(result.IsError); // Kiểm tra giá trị là false
```

### Assert.Equal / Assert.NotEqual

```csharp
Assert.Equal("Expected", actual);        // Kiểm tra bằng nhau
Assert.NotEqual("Unexpected", actual);   // Kiểm tra không bằng
```

### Assert.Null / Assert.NotNull

```csharp
Assert.Null(result);      // Kiểm tra là null
Assert.NotNull(user);     // Kiểm tra không null
```

### Assert.Contains / Assert.DoesNotContain

```csharp
Assert.Contains("admin", users.Select(u => u.Username));  // Kiểm tra chứa phần tử
Assert.DoesNotContain("invalid", list);                   // Kiểm tra không chứa
```

### Assert.Throws / Assert.ThrowsAsync

```csharp
// Kiểm tra exception được throw
Assert.Throws<ArgumentException>(() => method());

// Kiểm tra async exception
await Assert.ThrowsAsync<InvalidOperationException>(async () => await methodAsync());
```

### Assert.All

```csharp
// Kiểm tra tất cả phần tử trong collection
Assert.All(users, user => Assert.NotEmpty(user.Username));
```

## Test Fixtures và Sharing Context

### IClassFixture

Sử dụng khi muốn share một instance giữa các tests trong cùng class:

```csharp
public class CoursesControllerTests : IClassFixture<WebApplicationFactoryHelper>
{
    private readonly WebApplicationFactoryHelper _factory;

    public CoursesControllerTests(WebApplicationFactoryHelper factory)
    {
        _factory = factory; // Instance được tạo một lần và dùng chung
    }
}
```

### Constructor và Dispose

```csharp
public class CourseRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public CourseRepositoryTests()
    {
        // Setup: Chạy trước mỗi test
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        // Cleanup: Chạy sau mỗi test
        _context?.Dispose();
    }
}
```

## Async Tests

XUnit hỗ trợ async/await:

```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
{
    var result = await _authService.LoginAsync(account);
    Assert.True(result.Success);
}
```

## Skip Tests

Bỏ qua một test tạm thời:

```csharp
[Fact(Skip = "Test này đang được sửa")]
public void TestThatIsBeingFixed()
{
    // ...
}
```

## Test Naming Convention

Tên test nên mô tả rõ ràng:
- **Format**: `MethodName_Scenario_ExpectedResult`
- **Ví dụ**: 
  - `LoginAsync_WithValidCredentials_ReturnsSuccess`
  - `RegisterAsync_WithExistingUsername_ReturnsFailure`
  - `GetByIdAsync_WithInvalidId_ReturnsNull`

## Best Practices

### 1. Arrange-Act-Assert (AAA) Pattern

```csharp
[Fact]
public void ExampleTest()
{
    // Arrange: Chuẩn bị dữ liệu và dependencies
    var service = new AuthService();
    var account = new UserAccount { Username = "test", Password = "pass" };
    
    // Act: Thực hiện hành động cần test
    var result = await service.LoginAsync(account);
    
    // Assert: Kiểm tra kết quả
    Assert.True(result.Success);
}
```

### 2. Test Independence

Mỗi test phải độc lập, không phụ thuộc vào test khác:

```csharp
// ✅ TỐT: Mỗi test tự setup data
[Fact]
public void Test1()
{
    var service = new AuthService();
    // Test logic
}

[Fact]
public void Test2()
{
    var service = new AuthService(); // Tạo mới, không dùng từ Test1
    // Test logic
}
```

### 3. One Assertion Per Test (lý tưởng)

```csharp
// ✅ TỐT: Một assertion rõ ràng
[Fact]
public void Test_ReturnsSuccess()
{
    var result = method();
    Assert.True(result.Success);
}

// ⚠️ CŨNG ĐƯỢC: Nhiều assertions nhưng cùng mục đích
[Fact]
public void Test_ReturnsValidUser()
{
    var user = GetUser();
    Assert.NotNull(user);
    Assert.Equal("admin", user.Username);
    Assert.Equal("Admin", user.Role);
}
```

### 4. Use Descriptive Names

```csharp
// ✅ TỐT
[Fact]
public void LoginAsync_WithInvalidPassword_ReturnsFailure()

// ❌ TỒI
[Fact]
public void Test1()
```

## Chạy XUnit Tests

### Command Line

```bash
# Chạy tất cả tests
dotnet test

# Chạy với output chi tiết
dotnet test --logger "console;verbosity=detailed"

# Chạy một test class
dotnet test --filter "FullyQualifiedName~AuthServiceTests"

# Chạy một test method cụ thể
dotnet test --filter "FullyQualifiedName~LoginAsync_WithValidCredentials_ReturnsSuccess"
```

### Visual Studio

1. Build solution (Ctrl+Shift+B)
2. Mở **Test Explorer** (Test → Test Explorer hoặc Ctrl+E, T)
3. Click **Run All** hoặc click vào test cụ thể

### Visual Studio Code

1. Cài extension **.NET Core Test Explorer**
2. Mở Command Palette (Ctrl+Shift+P)
3. Chọn **.NET: Run Tests**
4. Hoặc dùng terminal: `dotnet test`

## Các Test Attributes

| Attribute | Mô tả |
|-----------|-------|
| `[Fact]` | Đánh dấu một test method |
| `[Theory]` | Đánh dấu một test method nhận parameters |
| `[InlineData]` | Cung cấp dữ liệu cho Theory test |
| `[MemberData]` | Cung cấp dữ liệu từ property/method |
| `[ClassData]` | Cung cấp dữ liệu từ class |
| `[Fact(Skip="reason")]` | Bỏ qua test này |

## Ví Dụ Thực Tế trong Project

### Example 1: Simple Unit Test

```csharp
[Fact]
public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
{
    // Arrange
    var account = new UserAccount
    {
        Username = "admin",
        Password = "password"
    };

    // Act
    var result = await _authService.LoginAsync(account);

    // Assert
    Assert.True(result.Success);
    Assert.Equal("Login successful!", result.Message);
}
```

### Example 2: Theory Test

```csharp
[Theory]
[InlineData("admin", "password", true)]
[InlineData("admin", "wrong", false)]
[InlineData("invalid", "password", false)]
public async Task LoginAsync_WithVariousCredentials_ReturnsExpected(
    string username, string password, bool expectedSuccess)
{
    var account = new UserAccount { Username = username, Password = password };
    var result = await _authService.LoginAsync(account);
    Assert.Equal(expectedSuccess, result.Success);
}
```

### Example 3: Integration Test với Fixture

```csharp
public class CoursesControllerTests : IClassFixture<WebApplicationFactoryHelper>
{
    private readonly WebApplicationFactoryHelper _factory;

    public CoursesControllerTests(WebApplicationFactoryHelper factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Index_Get_ReturnsViewWithCourses()
    {
        // Arrange
        await CreateTestCourseAsync(_factory.Services);
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Courses");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Courses", content);
    }
}
```

## Tài Liệu Tham Khảo

- [XUnit Documentation](https://xunit.net/)
- [XUnit GitHub](https://github.com/xunit/xunit)
- [.NET Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/)

## Tổng Kết

- ✅ Sử dụng `[Fact]` cho test đơn giản
- ✅ Sử dụng `[Theory]` + `[InlineData]` cho test với nhiều input
- ✅ Tuân theo AAA pattern (Arrange-Act-Assert)
- ✅ Đặt tên test mô tả rõ ràng
- ✅ Mỗi test phải độc lập
- ✅ Sử dụng assertions phù hợp
