using Microsoft.Win32;
using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Events;
using NickvisionMoney.Shared.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : UserControl, INotifyPropertyChanged
{
    internal readonly AccountViewController _controller;
    private ObservableCollection<Transaction> _transactions;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set
        {
            _transactions = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transactions)));
        }
    }

    public AccountView(AccountViewController controller)
    {
        InitializeComponent();
        _controller = controller;
        _transactions = new ObservableCollection<Transaction>(_controller.Transactions.Values);
        DataContext = this;

        // Set account information
        AccountNameText.Text = _controller.AccountTitle;
        AccountBalanceText.Text = _controller.AccountTodayTotal.ToString("C");

        // Subscribe to events
        _controller.TransactionCreated += OnTransactionCreated;
        _controller.TransactionUpdated += OnTransactionUpdated;
        _controller.TransactionDeleted += OnTransactionDeleted;
    }

    private async void AddTransaction_Click(object sender, RoutedEventArgs e)
    {
        var controller = _controller.CreateTransactionDialogController();
        var dialog = new Dialogs.TransactionDialog(controller);
        
        if (dialog.ShowDialog() == true && dialog.WasSaved)
        {
            await _controller.AddTransactionAsync(controller.Transaction);
        }
    }

    private void Transfer_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement TransferDialog
        MessageBox.Show("Transfer dialog will be implemented.", "Transfer", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void AccountSettings_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Implement AccountSettingsDialog
        MessageBox.Show("Account settings dialog will be implemented.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void SearchBox_TextChanged(object sender, RoutedEventArgs e)
    {
        var searchText = SearchBox.Text?.ToLower() ?? "";
        var filteredTransactions = _controller.Transactions.Values
            .Where(t => t.Description.ToLower().Contains(searchText))
            .ToList();
        
        Transactions = new ObservableCollection<Transaction>(filteredTransactions);
    }

    private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FilterComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var filter = selectedItem.Content.ToString();
            var filteredTransactions = filter switch
            {
                "Income" => _controller.Transactions.Values.Where(t => t.Amount > 0),
                "Expense" => _controller.Transactions.Values.Where(t => t.Amount < 0),
                "This Month" => _controller.Transactions.Values.Where(t => t.Date.Month == DateTime.Now.Month && t.Date.Year == DateTime.Now.Year),
                "Last Month" => _controller.Transactions.Values.Where(t => t.Date.Month == DateTime.Now.AddMonths(-1).Month && t.Date.Year == DateTime.Now.AddMonths(-1).Year),
                "This Year" => _controller.Transactions.Values.Where(t => t.Date.Year == DateTime.Now.Year),
                _ => _controller.Transactions.Values
            };
            
            Transactions = new ObservableCollection<Transaction>(filteredTransactions);
        }
    }

    private void Export_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Export Account",
            Filter = "PDF Files (*.pdf)|*.pdf|CSV Files (*.csv)|*.csv|QIF Files (*.qif)|*.qif|OFX Files (*.ofx)|*.ofx",
            DefaultExt = ".pdf",
            FileName = $"{_controller.AccountTitle}_export"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            // TODO: Implement export functionality
            MessageBox.Show("Export functionality will be implemented based on the selected format.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private async void TransactionsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (TransactionsListView.SelectedItem is Transaction transaction)
        {
            var controller = _controller.CreateTransactionDialogController(transaction.Id);
            var dialog = new Dialogs.TransactionDialog(controller);
            
            if (dialog.ShowDialog() == true && dialog.WasSaved)
            {
                await _controller.UpdateTransactionAsync(controller.Transaction);
            }
        }
    }

    private void TransactionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Handle selection changes if needed
    }

    private void OnTransactionCreated(object? sender, ModelEventArgs<Transaction> e)
    {
        Transactions = new ObservableCollection<Transaction>(_controller.Transactions.Values);
        AccountBalanceText.Text = _controller.AccountTodayTotal.ToString("C");
    }

    private void OnTransactionUpdated(object? sender, ModelEventArgs<Transaction> e)
    {
        Transactions = new ObservableCollection<Transaction>(_controller.Transactions.Values);
        AccountBalanceText.Text = _controller.AccountTodayTotal.ToString("C");
    }

    private void OnTransactionDeleted(object? sender, uint e)
    {
        Transactions = new ObservableCollection<Transaction>(_controller.Transactions.Values);
        AccountBalanceText.Text = _controller.AccountTodayTotal.ToString("C");
    }
} 