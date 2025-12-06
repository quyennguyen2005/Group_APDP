# PowerShell script để chạy automation tests

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "  CHẠY AUTOMATION TESTS" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

# Build project
Write-Host "1. Building project..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Please fix errors before running tests." -ForegroundColor Red
    exit 1
}
Write-Host "Build successful!" -ForegroundColor Green
Write-Host ""

# Chạy tests
Write-Host "2. Running all tests..." -ForegroundColor Yellow
Write-Host ""

# Option 1: Chạy tất cả tests
$choice = Read-Host "Chọn loại test (1=All, 2=Unit Tests only, 3=Integration Tests only, 4=Specific class): "

switch ($choice) {
    "1" {
        Write-Host "Running ALL tests..." -ForegroundColor Cyan
        dotnet test --logger "console;verbosity=normal"
    }
    "2" {
        Write-Host "Running UNIT TESTS only..." -ForegroundColor Cyan
        dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests" --logger "console;verbosity=normal"
    }
    "3" {
        Write-Host "Running INTEGRATION TESTS only..." -ForegroundColor Cyan
        dotnet test --filter "FullyQualifiedName~AccountControllerTests|CoursesControllerTests" --logger "console;verbosity=normal"
    }
    "4" {
        Write-Host ""
        Write-Host "Available test classes:" -ForegroundColor Cyan
        Write-Host "  1. AuthServiceTests"
        Write-Host "  2. CourseRepositoryTests"
        Write-Host "  3. AccountControllerTests"
        Write-Host "  4. CoursesControllerTests"
        Write-Host ""
        $testClass = Read-Host "Enter test class name (e.g., AuthServiceTests): "
        dotnet test --filter "FullyQualifiedName~$testClass" --logger "console;verbosity=normal"
    }
    default {
        Write-Host "Running ALL tests (default)..." -ForegroundColor Cyan
        dotnet test --logger "console;verbosity=normal"
    }
}

Write-Host ""
Write-Host "======================================" -ForegroundColor Cyan
Write-Host "  TEST RUN COMPLETED" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
