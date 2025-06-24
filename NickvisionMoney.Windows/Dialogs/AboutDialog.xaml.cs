using ModernWpf.Controls;
using Nickvision.Aura;
using System.Linq;

namespace NickvisionMoney.Windows.Views;

/// <summary>
/// Interaction logic for AboutDialog.xaml
/// </summary>
public partial class AboutDialog : ContentDialog
{
    public AboutDialog(AppInfo appInfo)
    {
        InitializeComponent();
        
        AppNameText.Text = appInfo.ShortName;
        VersionText.Text = $"Version {appInfo.Version}";
        DescriptionText.Text = appInfo.Description;
        ChangelogText.Text = appInfo.Changelog;
        
        // Format developers
        var developers = string.Join(", ", appInfo.Developers.Keys);
        DevelopersText.Text = developers;
    }
} 