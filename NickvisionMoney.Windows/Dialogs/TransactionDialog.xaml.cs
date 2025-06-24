using NickvisionMoney.Shared.Controllers;
using NickvisionMoney.Shared.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace NickvisionMoney.Windows.Dialogs;

/// <summary>
/// Interaction logic for TransactionDialog.xaml
/// </summary>
public partial class TransactionDialog : Window
{
    public TransactionDialogController Controller { get; private set; }
    public bool WasSaved { get; private set; }

    public TransactionDialog(TransactionDialogController controller)
    {
        InitializeComponent();
        Controller = controller;
        WasSaved = false;
        
        LoadData();
    }

    private void LoadData()
    {
        // Load group names
        GroupComboBox.ItemsSource = Controller.GroupNames;
        if (Controller.GroupNames.Count > 0)
        {
            GroupComboBox.SelectedIndex = 0;
        }

        // Set defaults or load existing transaction data
        if (Controller.IsEditing)
        {
            Title = "Edit Transaction";
            DescriptionTextBox.Text = Controller.Transaction.Description;
            AmountTextBox.Text = Math.Abs(Controller.Transaction.Amount).ToString("F2", Controller.CultureForNumberString);
            DatePicker.SelectedDate = Controller.Transaction.Date.ToDateTime(TimeOnly.MinValue);
            TypeComboBox.SelectedIndex = Controller.Transaction.Amount >= 0 ? 0 : 1; // 0 = Income, 1 = Expense
            
            // Set group
            var groupName = Controller.GetGroupNameFromId((uint)Math.Max(0, Controller.Transaction.GroupId));
            var groupIndex = Controller.GroupNames.IndexOf(groupName);
            if (groupIndex >= 0)
            {
                GroupComboBox.SelectedIndex = groupIndex;
            }
            
            // Set color
            ColorTextBox.Text = Controller.Transaction.RGBA;
            UpdateColorButton(Controller.Transaction.RGBA);
            
            // Set repeat info
            RepeatCheckBox.IsChecked = Controller.Transaction.RepeatInterval != TransactionRepeatInterval.Never;
            RepeatTypeComboBox.SelectedIndex = (int)Controller.Transaction.RepeatInterval;
            if (Controller.Transaction.RepeatEndDate.HasValue)
            {
                RepeatEndDatePicker.SelectedDate = Controller.Transaction.RepeatEndDate.Value.ToDateTime(TimeOnly.MinValue);
            }
            
            // Set tags
            TagsTextBox.Text = string.Join(", ", Controller.Transaction.Tags);
        }
        else
        {
            Title = "Add Transaction";
            DatePicker.SelectedDate = DateTime.Today;
            ColorTextBox.Text = Controller.Transaction.RGBA;
            UpdateColorButton(Controller.Transaction.RGBA);
        }
    }

    private void UpdateColorButton(string rgba)
    {
        try
        {
            var color = (Color)ColorConverter.ConvertFromString(rgba);
            ColorButton.Background = new SolidColorBrush(color);
        }
        catch
        {
            ColorButton.Background = new SolidColorBrush(Colors.Blue);
        }
    }

    private void ColorButton_Click(object sender, RoutedEventArgs e)
    {
        // Simple color picker - in a real implementation you'd want a proper color picker dialog
        var colors = new[] { "#FF0000FF", "#FFFF0000", "#FF00FF00", "#FFFFFF00", "#FFFF00FF", "#FF00FFFF", "#FFFFA500", "#FF800080" };
        var currentIndex = Array.IndexOf(colors, ColorTextBox.Text);
        var nextIndex = (currentIndex + 1) % colors.Length;
        ColorTextBox.Text = colors[nextIndex];
        UpdateColorButton(colors[nextIndex]);
    }

    private void RepeatCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        RepeatPanel.Visibility = Visibility.Visible;
    }

    private void RepeatCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        RepeatPanel.Visibility = Visibility.Collapsed;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
        {
            MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountTextBox.Text, NumberStyles.Currency, Controller.CultureForNumberString, out decimal amount))
        {
            MessageBox.Show("Please enter a valid amount.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!DatePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Please select a date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Get values from form
        var date = DateOnly.FromDateTime(DatePicker.SelectedDate.Value);
        var description = DescriptionTextBox.Text;
        var type = TypeComboBox.SelectedIndex == 1 ? TransactionType.Expense : TransactionType.Income;
        var selectedRepeat = RepeatCheckBox.IsChecked == true ? RepeatTypeComboBox.SelectedIndex : 0; // 0 = Never
        var groupName = GroupComboBox.SelectedItem?.ToString() ?? "Ungrouped";
        var rgba = ColorTextBox.Text;
        var useGroupColor = false; // For now, always use transaction color
        var tags = TagsTextBox.Text.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
        var amountString = amount.ToString("F2", Controller.CultureForNumberString);
        var repeatEndDate = RepeatEndDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(RepeatEndDatePicker.SelectedDate.Value) : (DateOnly?)null;
        var notes = ""; // Not implemented in this simple dialog
        
        // Update the transaction using the controller's method
        var result = Controller.UpdateTransaction(date, description, type, selectedRepeat, groupName, rgba, useGroupColor, tags, amountString, null, repeatEndDate, notes);
        
        if (result != TransactionCheckStatus.Valid)
        {
            var errorMessage = "Transaction validation failed:";
            if (result.HasFlag(TransactionCheckStatus.EmptyDescription))
                errorMessage += "\n• Description is required";
            if (result.HasFlag(TransactionCheckStatus.InvalidAmount))
                errorMessage += "\n• Invalid amount";
            if (result.HasFlag(TransactionCheckStatus.InvalidRepeatEndDate))
                errorMessage += "\n• Repeat end date must be after the transaction date";
            
            MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        WasSaved = true;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
} 