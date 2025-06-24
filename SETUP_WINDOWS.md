# Windows Application Setup Guide

## 🚨 Prerequisites Missing

Your system doesn't have the .NET SDK installed, which is required to build and run the Windows application. Here's how to get everything set up:

## Option 1: Install .NET SDK (Recommended)

### Step 1: Download .NET 9.0 SDK
1. Go to: https://dotnet.microsoft.com/download/dotnet/9.0
2. Download the **SDK** (not just Runtime) for Windows x64
3. Choose the installer (.exe) version for easier setup

### Step 2: Install .NET SDK
1. Run the downloaded installer
2. Follow the installation wizard
3. Restart your command prompt/PowerShell after installation

### Step 3: Verify Installation
Open a new PowerShell window and run:
```powershell
dotnet --version
```
You should see something like `9.0.xxx`

### Step 4: Build the Windows Application
```powershell
cd "C:\Users\panas\Documents\GitHub\Denaro"
dotnet build NickvisionMoney.Windows
```

### Step 5: Run the Application
```powershell
dotnet run --project NickvisionMoney.Windows
```

## Option 2: Use Visual Studio 2022

### Step 1: Download Visual Studio 2022
1. Go to: https://visualstudio.microsoft.com/downloads/
2. Download **Community** edition (free)
3. During installation, make sure to select:
   - **.NET desktop development** workload
   - **Windows Presentation Foundation** components

### Step 2: Open the Project
1. Launch Visual Studio 2022
2. Open the solution file: `Denaro.sln`
3. Set `NickvisionMoney.Windows` as the startup project:
   - Right-click on `NickvisionMoney.Windows` in Solution Explorer
   - Select "Set as Startup Project"

### Step 3: Build and Run
1. Press **F5** to build and run with debugging
2. Or press **Ctrl+F5** to run without debugging

## Option 3: Alternative Build Methods

### Using MSBuild (if you have it)
```powershell
# Find MSBuild
where msbuild

# If found, build with:
msbuild NickvisionMoney.Windows\NickvisionMoney.Windows.csproj
```

### Using Visual Studio Developer Command Prompt
1. Search for "Developer Command Prompt" in Start Menu
2. If found, navigate to the project directory
3. Run: `msbuild NickvisionMoney.Windows\NickvisionMoney.Windows.csproj`

## 🔧 Quick Setup Commands

After installing .NET SDK, here are the commands to get running quickly:

```powershell
# Navigate to project directory
cd "C:\Users\panas\Documents\GitHub\Denaro"

# Restore NuGet packages
dotnet restore

# Build the Windows application
dotnet build NickvisionMoney.Windows

# Run the application
dotnet run --project NickvisionMoney.Windows

# Or publish for distribution
dotnet publish NickvisionMoney.Windows -c Release -o .\publish-windows
```

## 📁 What You'll Get

Once running, the Windows application will provide:

### Modern Windows Interface
- **Fluent Design**: Native Windows 11/10 styling
- **Dark/Light Themes**: Follows system preferences
- **High DPI Support**: Scales properly on 4K displays

### Full Feature Set
- **Account Management**: Create, open, manage multiple accounts
- **Transaction Tracking**: Add, edit, search, filter transactions
- **Categories**: Color-coded transaction categories
- **Import/Export**: CSV, QIF, OFX file support
- **Encryption**: Password-protected accounts
- **Cross-Platform**: Same files work on Linux/GNOME version

### Screenshots Preview
```
┌─────────────────────────────────────────────────────────┐
│ 📁 File  ❓ Help                                        │
├─────────────────┬───────────────────────────────────────┤
│ Recent Accounts │ 👋 Good [Morning/Afternoon/Evening]! │
│ 📄 My Checking  │                                       │
│ 💰 Savings      │ Welcome to Denaro                     │
│ 🏢 Business     │                                       │
│                 │ [Create New Account] [Open Account]   │
│                 │                                       │
└─────────────────┴───────────────────────────────────────┘
```

## 🛠️ Troubleshooting

### Issue: "dotnet not recognized"
**Solution**: Install .NET 9.0 SDK from Microsoft's website

### Issue: "Package not found" errors
**Solution**: Run `dotnet restore` in the project directory

### Issue: "ModernWpf" errors
**Solution**: Ensure you're targeting Windows and have restored packages

### Issue: Application won't start
**Solution**: Check that you're running on Windows 10 1809 or later

### Issue: Missing icon/resources
**Solution**: The icon files are placeholders - copy real icons from the shared project

## 🎯 Next Steps After Setup

1. **Test the Application**: Create a test account and add some transactions
2. **Import Existing Data**: If you have financial data, test the import features
3. **Customize Settings**: Explore preferences for themes and currencies
4. **Compare with GNOME**: If you have the Linux version, verify file compatibility

## 📞 Support

If you encounter issues:
1. Check the error messages carefully
2. Ensure all prerequisites are installed
3. Try building with Visual Studio for better error reporting
4. Check the GitHub repository for similar issues

The Windows application provides the same powerful financial management features as the GNOME version, but with a native Windows experience! 