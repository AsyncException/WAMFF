using Microsoft.UI.Xaml.Controls;
using WAMFF.Core.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WAMFF.Components;

public partial class CategoryDisplayCollection : UserControl
{
    private CategoryDisplayCollectionViewModel ViewModel { get; } = new();

    public CategoryDisplayCollection() {
        this.InitializeComponent();
        ViewModel.CreateDialog = CreateDialog;
    }
}