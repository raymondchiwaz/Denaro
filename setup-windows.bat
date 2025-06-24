@echo off
echo ====================================================
echo Denaro Windows Application Setup
echo ====================================================
echo.

echo Checking for .NET SDK...
dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo.
    echo Please install .NET 9.0 SDK from:
    echo https://dotnet.microsoft.com/download/dotnet/9.0
    echo.
    echo After installation, restart this script.
    pause
    exit /b 1
)

echo .NET SDK found! Version:
dotnet --version
echo.

echo Restoring NuGet packages...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo Building Windows application...
dotnet build NickvisionMoney.Windows
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo ====================================================
echo BUILD SUCCESSFUL!
echo ====================================================
echo.
echo To run the application:
echo   dotnet run --project NickvisionMoney.Windows
echo.
echo Or run the published version:
echo   dotnet publish NickvisionMoney.Windows -c Release -o .\publish-windows
echo   .\publish-windows\NickvisionMoney.Windows.exe
echo.
pause 