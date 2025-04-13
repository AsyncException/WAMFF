using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WAMFF.Core.Services;
using WAMFF.Core.ViewModels;

namespace WAMFF.Components;

public partial class FileDisplay : UserControl
{
    public FileDisplayViewModel ViewModel {
        get {
            return (FileDisplayViewModel)GetValue(ViewModelProperty);
        }

        set {
            value.CategoryDialog = CategoryDialog;
            value.DeleteDialog = DeleteDialog;
            value.RenameDialog = RenameDialog;
            value.TagsDialog = TagsDialog;
            SetValue(ViewModelProperty, value);
        }
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(FileDisplayViewModel), typeof(FileDisplay), null);

    public FileDisplay() {
        this.InitializeComponent();
        VSCodeFlyout.Visibility = ConfigurationProvider.CurrentConfig.IsVSCodeInstalled ? Visibility.Visible : Visibility.Collapsed;
    }
}