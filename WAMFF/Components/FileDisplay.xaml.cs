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
        this.SizeChanged += Window_SizeChanged;
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
        TypeColumn.Width = this.NameColumn switch {
            { ActualWidth: < 200 } => new GridLength(0),
            { ActualWidth: > 400 } => new GridLength(200),
            _ => TypeColumn.Width
        };
    }
}