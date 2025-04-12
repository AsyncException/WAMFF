using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WAMFF.Core.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WAMFF.Components;

public partial class CategoryDisplay : UserControl
{
    public CategoryDisplayViewModel ViewModel {
        get {
            return (CategoryDisplayViewModel)GetValue(ViewModelProperty);
        }

        set {
            value.EditDialog = EditDialog;
            value.DeleteDialog = DeleteDialog;
            SetValue(ViewModelProperty, value);
        }
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(FileDisplayViewModel), typeof(FileDisplay), null);

    public CategoryDisplay() {
        this.InitializeComponent();
    }
}