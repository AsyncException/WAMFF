using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WAMFF.Components;

public partial class FileDisplayColletion : UserControl
{
    public FileDisplayColletion() {
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