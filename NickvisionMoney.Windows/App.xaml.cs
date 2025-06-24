using ModernWpf;
using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Models;
using System.Globalization;
using System.Windows;

namespace NickvisionMoney.Windows;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainWindowController? _mainWindowController;

    protected override void OnStartup(StartupEventArgs e)
    {
        // Set culture info like in the GNOME version
        if (CultureInfo.CurrentCulture.Equals(CultureInfo.InvariantCulture))
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }
        else if (CultureInfo.CurrentCulture.ToString() == "ar-RG")
        {
            CultureInfo.CurrentCulture = new CultureInfo("ar-EG");
        }

        // Initialize the main window controller
        _mainWindowController = new MainWindowController(e.Args);
        _mainWindowController.AppInfo.Changelog =
            @"* Updated dependencies
              * Fixed an icon conflict with GNOME System Monitor
              * Fixed an issue where repeated transactions would not be updated correctly after updating the source transaction
              * Updated and added translations (Thanks to everyone on Weblate)!";

        // Set ModernWPF theme based on configuration
        ThemeManager.Current.ApplicationTheme = _mainWindowController.Theme switch
        {
            Theme.System => null, // Use system theme
            Theme.Light => ApplicationTheme.Light,
            Theme.Dark => ApplicationTheme.Dark,
            _ => null
        };

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mainWindowController?.Dispose();
        base.OnExit(e);
    }
} 