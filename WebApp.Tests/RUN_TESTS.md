# Hướng Dẫn Chạy Automation Tests

## Các Cách Chạy Tests

### 1. Chạy TẤT CẢ Tests

Mở terminal/PowerShell tại thư mục gốc dự án và chạy:

```bash
dotnet test
```

Hoặc chỉ định rõ project:

```bash
dotnet test WebApp.Tests/WebApp.Tests.csproj
```

### 2. Chạy Tests Với Output Chi Tiết

```bash
dotnet test --logger "console;verbosity=detailed"
```

### 3. Chạy Tests Và Hiển Thị Output Trong Quá Trình Test

```bash
dotnet test --logger "console;verbosity=normal"
```

### 4. Chạy Một Test Class Cụ Thể

```bash
# Chạy tất cả tests trong AuthServiceTests
dotnet test --filter "FullyQualifiedName~AuthServiceTests"

# Chạy tất cả tests trong CourseRepositoryTests
dotnet test --filter "FullyQualifiedName~CourseRepositoryTests"

# Chạy tất cả tests trong AccountControllerTests
dotnet test --filter "FullyQualifiedName~AccountControllerTests"

# Chạy tất cả tests trong CoursesControllerTests
dotnet test --filter "FullyQualifiedName~CoursesControllerTests"
```

### 5. Chạy Một Test Method Cụ Thể

```bash
# Ví dụ: chạy test LoginAsync_WithValidCredentials_ReturnsSuccess
dotnet test --filter "FullyQualifiedName~LoginAsync_WithValidCredentials_ReturnsSuccess"
```

### 6. Chạy Chỉ Unit Tests (Đã Hoạt Động Tốt)

```bash
dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests"
```

### 7. Chạy Tests và Tạo Báo Cáo

```bash
# Tạo báo cáo dạng trx (Visual Studio Test Results)
dotnet test --logger "trx;LogFileName=test-results.trx"

# Tạo báo cáo dạng HTML
dotnet test --logger "html;LogFileName=test-results.html"
```

### 8. Chạy Tests và Xem Code Coverage (nếu có cài coverlet)

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Chạy Tests Trong Visual Studio

1. Mở Solution trong Visual Studio
2. Mở **Test Explorer** (View → Test Explorer hoặc Ctrl+E, T)
3. Build solution (Ctrl+Shift+B)
4. Click **Run All Tests** hoặc click vào test cụ thể để chạy

## Chạy Tests Trong Visual Studio Code

1. Cài extension: **.NET Core Test Explorer** hoặc **C#**
2. Mở Command Palette (Ctrl+Shift+P)
3. Chạy lệnh: **.NET: Run Tests**
4. Hoặc sử dụng terminal tích hợp và chạy `dotnet test`

## Kiểm Tra Kết Quả Test

Sau khi chạy test, bạn sẽ thấy kết quả như sau:

```
Passed!  - Failed:     0, Passed:    27, Skipped:     0, Total:    27
```

- **Passed**: Số lượng test pass
- **Failed**: Số lượng test fail
- **Total**: Tổng số test
- **Duration**: Thời gian chạy

## Các Test Đã Được Tạo

### ✅ Unit Tests (Hoạt động tốt)
- **AuthServiceTests**: 12 tests - Test authentication service
- **CourseRepositoryTests**: 6 tests - Test CRUD operations

### ⚠️ Integration Tests (Một số cần fix database config)
- **AccountControllerTests**: 6 tests - Test login/register pages
- **CoursesControllerTests**: 9 tests - Test course management

## Troubleshooting

### Lỗi Database Configuration

Nếu gặp lỗi về database providers, có thể chạy riêng unit tests:

```bash
dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests"
```

### Lỗi Build

Đảm bảo đã restore packages:

```bash
dotnet restore
dotnet build
dotnet test
```

### Xem Log Chi Tiết Khi Test Fail

```bash
dotnet test --logger "console;verbosity=detailed" --verbosity detailed
```

## Scripts Hữu Ích

Tạo file `run-tests.ps1` (PowerShell) hoặc `run-tests.sh` (Bash) để chạy nhanh:

**PowerShell (run-tests.ps1)**:
```powershell
Write-Host "Building project..." -ForegroundColor Yellow
dotnet build

Write-Host "`nRunning all tests..." -ForegroundColor Yellow
dotnet test --logger "console;verbosity=normal"

Write-Host "`nTest run completed!" -ForegroundColor Green
```

**Bash (run-tests.sh)**:
```bash
#!/bin/bash
echo "Building project..."
dotnet build

echo "Running all tests..."
dotnet test --logger "console;verbosity=normal"

echo "Test run completed!"
```
