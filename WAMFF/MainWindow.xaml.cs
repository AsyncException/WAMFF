using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WAMFF.Core.ViewModels;
using WAMFF.Utilities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WAMFF;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly BackDropController f_BackDropController = Ioc.Default.GetRequiredService<BackDropController>()!;

    public MainWindowViewModel ViewModel { get; } = new();

    public MainWindow() {
        this.InitializeComponent();
        f_BackDropController.SetAcrylicBackdrop(this);
        ExtendsContentIntoTitleBar = true;
    }
}