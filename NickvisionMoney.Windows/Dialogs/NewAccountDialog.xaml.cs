using Microsoft.Win32;
using ModernWpf.Controls;
using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Models;
using System;
using System.IO;
using System.Windows;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for NewAccountDialog.xaml
/// </summary>
public partial class NewAccountDialog : ContentDialog
{
    public NewAccountDialogController? Controller { get; private set; }

    public NewAccountDialog(NewAccountDialogController controller)
    {
        InitializeComponent();
        Controller = controller;
        
        // Set default location
        var defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyAccount.nmoney");
        LocationTextBox.Text = defaultPath;
        
        PrimaryButtonClick += OnPrimaryButtonClick;
    }

    private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(AccountNameTextBox.Text))
        {
            args.Cancel = true;
            ShowError("Please enter an account name.");
            return;
        }

        if (string.IsNullOrWhiteSpace(LocationTextBox.Text))
        {
            args.Cancel = true;
            ShowError("Please choose a save location.");
            return;
        }

        if (PasswordProtectedCheckBox.IsChecked == true)
        {
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                args.Cancel = true;
                ShowError("Please enter a password.");
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                args.Cancel = true;
                ShowError("Passwords do not match.");
                return;
            }
        }

        // Set up the controller with the form data
        if (Controller != null)
        {
            // Validate and set name
            var nameStatus = Controller.UpdateName(AccountNameTextBox.Text);
            if (nameStatus.HasFlag(NameCheckStatus.AlreadyOpen))
            {
                args.Cancel = true;
                ShowError("An account with this name is already open. Please choose a different name.");
                return;
            }
            if (nameStatus.HasFlag(NameCheckStatus.Exists))
            {
                args.Cancel = true;
                ShowError("An account with this name already exists. Please choose a different name or enable overwrite.");
                return;
            }

            // Set folder from the selected path
            Controller.Folder = Path.GetDirectoryName(LocationTextBox.Text) ?? "";
            
            // Set account type
            Controller.Metadata.AccountType = AccountTypeComboBox.SelectedIndex switch
            {
                0 => AccountType.Checking,
                1 => AccountType.Savings,
                2 => AccountType.Business,
                _ => AccountType.Checking
            };
            
            // Set password if enabled
            Controller.Password = PasswordProtectedCheckBox.IsChecked == true ? PasswordBox.Password : null;
            
            // Set import file if enabled
            Controller.ImportFile = ImportDataCheckBox.IsChecked == true ? ImportFileTextBox.Text : "";
        }
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save Account As",
            Filter = "Denaro Account Files (*.nmoney)|*.nmoney",
            DefaultExt = ".nmoney",
            FileName = AccountNameTextBox.Text ?? "MyAccount"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            LocationTextBox.Text = saveFileDialog.FileName;
        }
    }

    private void BrowseImportButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Select Import File",
            Filter = "All Supported Files|*.csv;*.qif;*.ofx|CSV Files (*.csv)|*.csv|QIF Files (*.qif)|*.qif|OFX Files (*.ofx)|*.ofx|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            ImportFileTextBox.Text = openFileDialog.FileName;
        }
    }

    private void PasswordProtectedCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        PasswordPanel.Visibility = Visibility.Visible;
    }

    private void PasswordProtectedCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        PasswordPanel.Visibility = Visibility.Collapsed;
        PasswordBox.Clear();
        ConfirmPasswordBox.Clear();
    }

    private void ImportDataCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ImportPanel.Visibility = Visibility.Visible;
    }

    private void ImportDataCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ImportPanel.Visibility = Visibility.Collapsed;
        ImportFileTextBox.Clear();
    }
} 