using ModernWpf.Controls;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for PasswordDialog.xaml
/// </summary>
public partial class PasswordDialog : ContentDialog
{
    public string? Password => PasswordBox.Password;

    public PasswordDialog(string accountName)
    {
        InitializeComponent();
        AccountNameText.Text = accountName;
    }
} 