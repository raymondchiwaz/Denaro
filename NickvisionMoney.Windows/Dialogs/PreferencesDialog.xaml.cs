using ModernWpf.Controls;
using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Models;
using System.Windows;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for PreferencesDialog.xaml
/// </summary>
public partial class PreferencesDialog : ContentDialog
{
    private readonly PreferencesViewController _controller;

    public PreferencesDialog(PreferencesViewController controller)
    {
        InitializeComponent();
        _controller = controller;
        
        LoadSettings();
        PrimaryButtonClick += OnPrimaryButtonClick;
    }

    private void LoadSettings()
    {
        ThemeComboBox.SelectedIndex = _controller.Theme switch
        {
            Theme.System => 0,
            Theme.Light => 1,
            Theme.Dark => 2,
            _ => 0
        };
        
        UseNativeDigitsCheckBox.IsChecked = _controller.UseNativeDigits;
        ShowGraphsCheckBox.IsChecked = true; // This is a main window controller property, not preferences
        
        // Set currency and colors based on controller values if available
        // For now, using defaults
    }

    private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        _controller.Theme = ThemeComboBox.SelectedIndex switch
        {
            0 => Theme.System,
            1 => Theme.Light,
            2 => Theme.Dark,
            _ => Theme.System
        };
        
        _controller.UseNativeDigits = UseNativeDigitsCheckBox.IsChecked ?? false;
        // ShowGraphs is handled by main window controller, not preferences
        
        _controller.SaveConfiguration();
    }

    private void CheckingColorButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement color picker
        MessageBox.Show("Color picker functionality will be implemented.", "Color Picker", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void SavingsColorButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement color picker
        MessageBox.Show("Color picker functionality will be implemented.", "Color Picker", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BusinessColorButton_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement color picker
        MessageBox.Show("Color picker functionality will be implemented.", "Color Picker", MessageBoxButton.OK, MessageBoxImage.Information);
    }
} 