@echo off
echo Starting Denaro Windows Application...
echo.

dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK is not installed
    echo Please run setup-windows.bat first
    pause
    exit /b 1
)

echo Running application...
dotnet run --project NickvisionMoney.Windows 