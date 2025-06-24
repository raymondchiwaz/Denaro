using Microsoft.Win32;
using ModernWpf.Controls;
using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly MainWindowController _controller;
    private ObservableCollection<RecentAccount> _recentAccounts;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<RecentAccount> RecentAccounts
    {
        get => _recentAccounts;
        set
        {
            _recentAccounts = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentAccounts)));
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        
        // Get the controller from the current application
        var app = Application.Current as App;
        _controller = new MainWindowController(Environment.GetCommandLineArgs().Skip(1).ToArray());
        
        // Set up password callback
        _controller.AccountLoginAsync = ShowPasswordDialog;
        
        // Initialize data binding
        _recentAccounts = new ObservableCollection<RecentAccount>(_controller.RecentAccounts);
        DataContext = this;
        
        // Set greeting text
        GreetingText.Text = _controller.Greeting;
        
        // Subscribe to events
        _controller.NotificationSent += OnNotificationSent;
        _controller.AccountAdded += OnAccountAdded;
        _controller.RecentAccountsChanged += OnRecentAccountsChanged;
        
        // Start the controller
        _ = StartupAsync();
    }

    private async Task StartupAsync()
    {
        await _controller.StartupAsync();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_controller.NumberOfOpenAccounts > 0)
        {
            WelcomePanel.Visibility = Visibility.Collapsed;
            AccountTabControl.Visibility = Visibility.Visible;
            UpdateAccountTabs();
        }
        else
        {
            WelcomePanel.Visibility = Visibility.Visible;
            AccountTabControl.Visibility = Visibility.Collapsed;
        }
    }

    private void UpdateAccountTabs()
    {
        // This method will be called from OnAccountAdded, so we don't need to rebuild all tabs
        // Just ensure the tab control is visible/hidden appropriately
    }

    private async Task<string?> ShowPasswordDialog(string accountName)
    {
        var dialog = new PasswordDialog(accountName);
        var result = await dialog.ShowAsync();
        return result == ContentDialogResult.Primary ? dialog.Password : null;
    }

    private async void NewAccount_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new NewAccountDialog(_controller.CreateNewAccountDialogController());
        var result = await dialog.ShowAsync();
        
        if (result == ContentDialogResult.Primary && dialog.Controller != null)
        {
            var success = await _controller.NewAccountAsync(dialog.Controller);
            if (success)
            {
                UpdateUI();
                StatusText.Text = "New account created successfully";
            }
        }
    }

    private async void OpenAccount_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Open Account File",
            Filter = "Denaro Account Files (*.nmoney)|*.nmoney|All Files (*.*)|*.*",
            DefaultExt = ".nmoney"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var success = await _controller.AddAccountAsync(openFileDialog.FileName);
            if (success)
            {
                UpdateUI();
                StatusText.Text = $"Account opened: {Path.GetFileNameWithoutExtension(openFileDialog.FileName)}";
            }
        }
    }

    private async void RecentAccountsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0 && e.AddedItems[0] is RecentAccount recentAccount)
        {
            var success = await _controller.AddAccountAsync(recentAccount.Path);
            if (success)
            {
                UpdateUI();
                StatusText.Text = $"Account opened: {recentAccount.Name}";
            }
            
            // Clear selection
            RecentAccountsListView.SelectedIndex = -1;
        }
    }

    private async void Preferences_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new PreferencesDialog(_controller.CreatePreferencesViewController());
        await dialog.ShowAsync();
    }

    private async void About_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AboutDialog(_controller.AppInfo);
        await dialog.ShowAsync();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnNotificationSent(object? sender, Nickvision.Aura.Events.NotificationSentEventArgs e)
    {
        StatusText.Text = e.Message;
    }

    private void OnAccountAdded(object? sender, EventArgs e)
    {
        // Create a new AccountView tab for the most recently added account
        var accountController = _controller.GetMostRecentAccountViewController();
        var accountView = new AccountView(accountController);
        var tabItem = new TabItem
        {
            Header = accountController.AccountTitle,
            Content = accountView
        };
        
        AccountTabControl.Items.Add(tabItem);
        AccountTabControl.SelectedItem = tabItem; // Select the new tab
        
        UpdateUI();
    }

    private void OnRecentAccountsChanged(object? sender, EventArgs e)
    {
        RecentAccounts = new ObservableCollection<RecentAccount>(_controller.RecentAccounts);
    }

    protected override void OnClosed(EventArgs e)
    {
        _controller?.Dispose();
        base.OnClosed(e);
    }
} 