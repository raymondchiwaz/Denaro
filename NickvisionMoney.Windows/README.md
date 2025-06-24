# Denaro Windows Application

This is the Windows WPF frontend for Denaro, a personal finance management application.

## Features

- Modern Windows UI using ModernWPF
- Full integration with the shared business logic
- Native Windows file dialogs and notifications
- Support for Windows themes (Light/Dark/System)
- High DPI awareness

## Requirements

- .NET 9.0 or later
- Windows 10 version 1809 (October 2018 Update) or later

## Building

To build the Windows application:

```bash
dotnet build NickvisionMoney.Windows
```

To run the application:

```bash
dotnet run --project NickvisionMoney.Windows
```

## Architecture

The Windows application follows the MVVM pattern and reuses all business logic from the `NickvisionMoney.Shared` project. The UI is built using:

- **WPF** for the UI framework
- **ModernWPF** for modern Windows styling
- **XAML** for declarative UI definitions

## Project Structure

- `Views/` - XAML views and code-behind files
- `Dialogs/` - Modal dialogs for user interaction  
- `Converters/` - Value converters for data binding
- `Resources/` - Application resources (icons, images, etc.)

## Key Components

- **MainWindow** - The main application window with sidebar and tab support
- **AccountView** - Displays account transactions and details
- **PasswordDialog** - Handles encrypted account authentication
- **AmountColorConverter** - Colors transaction amounts based on positive/negative values

## Notes

This Windows application provides the same functionality as the GNOME version but with a native Windows look and feel. All account files (.nmoney) are compatible between the GNOME and Windows versions. 