# 🚀 Hướng Dẫn Nhanh: Kiểm Tra Automation Tests

## ✅ Automation Tests Sử Dụng XUnit

Project này sử dụng **XUnit** làm testing framework để viết automation tests.

## 📋 Các Cách Kiểm Tra Tests

### **Cách 1: Chạy Tất Cả Tests (Đơn Giản Nhất)**

Mở PowerShell hoặc Terminal tại thư mục dự án, chạy:

```powershell
dotnet test
```

**Kết quả mẫu:**
```
Passed!  - Failed:     0, Passed:    27, Skipped:     0, Total:    27
```

### **Cách 2: Sử Dụng Script Có Sẵn**

Đã tạo sẵn script `run-tests.ps1` cho Windows:

```powershell
.\run-tests.ps1
```

Script sẽ hỏi bạn muốn chạy:
1. All tests
2. Unit Tests only (khuyên dùng - đã hoạt động tốt)
3. Integration Tests only
4. Specific class

### **Cách 3: Chạy Từng Loại Test**

#### Chạy Unit Tests (Đã hoạt động tốt - 18 tests):
```powershell
dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests"
```

#### Chạy Test Class Cụ Thể:
```powershell
# Test AuthService
dotnet test --filter "FullyQualifiedName~AuthServiceTests"

# Test CourseRepository
dotnet test --filter "FullyQualifiedName~CourseRepositoryTests"

# Test AccountController
dotnet test --filter "FullyQualifiedName~AccountControllerTests"

# Test CoursesController
dotnet test --filter "FullyQualifiedName~CoursesControllerTests"
```

#### Chạy Một Test Method Cụ Thể:
```powershell
dotnet test --filter "FullyQualifiedName~LoginAsync_WithValidCredentials_ReturnsSuccess"
```

### **Cách 4: Chạy Với Output Chi Tiết**

Xem thông tin chi tiết từng test:

```powershell
dotnet test --logger "console;verbosity=detailed"
```

### **Cách 5: Sử Dụng Visual Studio**

1. Mở file Solution (.sln) trong Visual Studio
2. Nhấn **Ctrl+Shift+B** để Build
3. Mở **Test Explorer**: 
   - Menu: `Test` → `Test Explorer`
   - Hoặc nhấn: `Ctrl+E, T`
4. Click **Run All** để chạy tất cả tests
5. Xem kết quả trong Test Explorer:
   - ✅ Xanh = Pass
   - ❌ Đỏ = Fail
   - ⚠️ Vàng = Skip

### **Cách 6: Sử Dụng Visual Studio Code**

1. Cài extension: **.NET Core Test Explorer** hoặc **C#**
2. Nhấn `Ctrl+Shift+P` → gõ `.NET: Run Tests`
3. Hoặc mở terminal tích hợp và chạy: `dotnet test`

## 📊 Kiểm Tra Kết Quả

### Kết Quả Thành Công:
```
Passed!  - Failed:     0, Passed:    27, Skipped:     0, Total:    27, Duration: 2 s
```

### Kết Quả Có Lỗi:
```
Failed!  - Failed:     2, Passed:    25, Skipped:     0, Total:    27
```

Xem chi tiết lỗi ở phần cuối output để biết test nào fail và lý do.

## 🎯 Test Coverage Hiện Tại

### ✅ Unit Tests (Hoạt động tốt):
- **AuthServiceTests**: 12 tests
  - Test login/register
  - Test password hashing
  - Test user retrieval
  
- **CourseRepositoryTests**: 6 tests
  - Test CRUD operations
  - Test data persistence

### ⚠️ Integration Tests (Một số cần fix):
- **AccountControllerTests**: 6 tests
  - Test login/register pages
  
- **CoursesControllerTests**: 9 tests
  - Test course management pages

## 🔍 Debugging Tests

### Xem Log Chi Tiết Khi Test Fail:

```powershell
dotnet test --logger "console;verbosity=detailed" --verbosity detailed
```

### Chạy Test và Dừng Ở Breakpoint:

1. Đặt breakpoint trong test code
2. Mở **Test Explorer** trong Visual Studio
3. Right-click vào test → **Debug Selected Tests**
4. Debugger sẽ dừng ở breakpoint

## 📝 Ví Dụ Test Output

```
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    12, Skipped:     0, Total:    12, Duration: 234 ms - WebApp.Tests.dll (net8.0)

Test Run Successful.
Total tests: 12
     Passed: 12
 Total time: 0.5 Seconds
```

## ⚡ Quick Commands

```powershell
# Build project
dotnet build

# Restore packages
dotnet restore

# Run all tests
dotnet test

# Run unit tests only (recommended)
dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## 📚 Tài Liệu Tham Khảo

- **XUnit Guide**: `WebApp.Tests/XUNIT_GUIDE.md` - Hướng dẫn chi tiết về XUnit
- **Run Tests Guide**: `WebApp.Tests/RUN_TESTS.md` - Hướng dẫn đầy đủ cách chạy tests
- **README**: `WebApp.Tests/README.md` - Tổng quan về test project

## 🎓 Best Practice

1. **Chạy tests thường xuyên** sau mỗi lần code
2. **Chạy tests trước khi commit** code
3. **Fix tests fail** trước khi merge code mới
4. **Chạy unit tests** trước vì chúng nhanh và ổn định hơn

## 💡 Tips

- Nếu gặp lỗi database, chạy riêng unit tests (cách 3)
- Nếu test fail, xem output chi tiết để biết nguyên nhân
- Sử dụng `--logger "console;verbosity=detailed"` để debug
- Trong Visual Studio, double-click vào failed test để xem chi tiết lỗi
