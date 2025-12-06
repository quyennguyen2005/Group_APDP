#!/bin/bash

# Bash script để chạy automation tests

echo "======================================"
echo "  CHẠY AUTOMATION TESTS"
echo "======================================"
echo ""

# Build project
echo "1. Building project..."
dotnet build
if [ $? -ne 0 ]; then
    echo "Build failed! Please fix errors before running tests."
    exit 1
fi
echo "Build successful!"
echo ""

# Chạy tests
echo "2. Running all tests..."
echo ""
echo "Chọn loại test:"
echo "  1. All tests"
echo "  2. Unit Tests only"
echo "  3. Integration Tests only"
echo "  4. Specific class"
echo ""
read -p "Your choice (1-4): " choice

case $choice in
    1)
        echo "Running ALL tests..."
        dotnet test --logger "console;verbosity=normal"
        ;;
    2)
        echo "Running UNIT TESTS only..."
        dotnet test --filter "FullyQualifiedName~AuthServiceTests|CourseRepositoryTests" --logger "console;verbosity=normal"
        ;;
    3)
        echo "Running INTEGRATION TESTS only..."
        dotnet test --filter "FullyQualifiedName~AccountControllerTests|CoursesControllerTests" --logger "console;verbosity=normal"
        ;;
    4)
        echo ""
        echo "Available test classes:"
        echo "  1. AuthServiceTests"
        echo "  2. CourseRepositoryTests"
        echo "  3. AccountControllerTests"
        echo "  4. CoursesControllerTests"
        echo ""
        read -p "Enter test class name: " testClass
        dotnet test --filter "FullyQualifiedName~$testClass" --logger "console;verbosity=normal"
        ;;
    *)
        echo "Running ALL tests (default)..."
        dotnet test --logger "console;verbosity=normal"
        ;;
esac

echo ""
echo "======================================"
echo "  TEST RUN COMPLETED"
echo "======================================"
