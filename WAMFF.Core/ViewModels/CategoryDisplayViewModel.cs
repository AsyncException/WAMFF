using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using WAMFF.Core.Converters;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.Services;
using Windows.UI;

namespace WAMFF.Core.ViewModels;

public partial class CategoryDisplayViewModel(Category category) : ObservableObject
{
    private readonly IFileRepository f_FileRepository = Ioc.Default.GetRequiredService<IFileRepository>();
    private readonly ICategoryRepository f_CategoryRepository = Ioc.Default.GetRequiredService<ICategoryRepository>();

    public Category Category { get; set; } = category;

    #region edit section

    public ContentDialog EditDialog { get; set; } = default!;

    [ObservableProperty] public partial string EditName { get; set; }
    [ObservableProperty] public partial Color EditColor { get; set; }

    [RelayCommand]
    public async Task EditCategory() {
        //User selected All or Uncatagoriesd defaults.
        if (Category == Category.All || Category == Category.Default || Category.Id is null) {
            return;
        }

        EditName = Category.Name;
        EditColor = Category.Color.FromHex();

        ContentDialogResult result = await EditDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            Category.Name = EditName;
            Category.Color = EditColor.ToString();
            f_CategoryRepository.Update(Category);
            StrongReferenceMessenger.Default.Send(new ForceCategoryRefreshMessage());
            StrongReferenceMessenger.Default.Send(new ForcedFileUpdateMessage());
        }
    }

    #endregion edit section

    #region deleting

    public ContentDialog DeleteDialog { get; set; } = default!;

    [RelayCommand]
    public async Task DeleteCategory() {
        if (Category == Category.All || Category == Category.Default || Category.Id is null) {
            return;
        }

        ContentDialogResult result = await DeleteDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            f_CategoryRepository.Delete(Category);
            f_FileRepository.CleanDeletedCategory(Category.Id!.Value);

            StrongReferenceMessenger.Default.Send(new ForceCategoryRefreshMessage());
            StrongReferenceMessenger.Default.Send(new ForcedFileUpdateMessage());
        }
    }

    #endregion deleting
}