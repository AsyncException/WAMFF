using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Helpers;
using LiteDB;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.Services;
using WAMFF.Core.Utilities;
using Windows.UI;

namespace WAMFF.Core.ViewModels;

public partial class CategoryDisplayCollectionViewModel : ObservableObject
{
    public CategoryDisplayCollectionViewModel() {
        StrongReferenceMessenger.Default.Register<CategoryDisplayCollectionViewModel, CategoryRequestMessage>(this, static (r, m) => {
            m.Reply(r.Categories.FirstOrDefault(e => e.Category.Id == m.CategoryId)?.Category ?? Category.Default);
        });

        StrongReferenceMessenger.Default.Register<CategoryDisplayCollectionViewModel, CategoriesRequestMessage>(this, static (r, m) => {
            m.Reply(r.Categories.Select(e => e.Category).ToList());
        });

        StrongReferenceMessenger.Default.Register<CategoryDisplayCollectionViewModel, ForceCategoryRefreshMessage>(this, static (r, m) => {
            r.RefreshCategories();
        });

        SelectedCategory = new(Category.All);
        RefreshCategories();
    }

    private readonly ICategoryRepository f_CategoryRepository = Ioc.Default.GetRequiredService<ICategoryRepository>();

    public ObservableCollection<CategoryDisplayViewModel> Categories { get; set; } = [];
    [ObservableProperty] public partial CategoryDisplayViewModel? SelectedCategory { get; set; }

    partial void OnSelectedCategoryChanged(CategoryDisplayViewModel? value) => StrongReferenceMessenger.Default.Send(new CategoryChangedMessage(value?.Category));

    public void RefreshCategories() {
        Categories.Replace([
            new(Category.All),
            new(Category.Default),
            ..f_CategoryRepository.GetAll().Select(e => new CategoryDisplayViewModel(e)).ToList()
            ]);
    }

    #region create category

    public ContentDialog CreateDialog { get; set; } = default!;
    [ObservableProperty] public partial string CreateName { get; set; } = string.Empty;
    [ObservableProperty] public partial Color CreateColor { get; set; } = Color.FromArgb(255, 255, 82, 159);

    [RelayCommand]
    public async Task CreateNewCategory() {
        CreateName = string.Empty;
        CreateColor = Color.FromArgb(255, 255, 82, 159);

        ContentDialogResult result = await CreateDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            Category category = new(CreateName, CreateColor.ToHex());
            f_CategoryRepository.Create(category);
            RefreshCategories();
        }
    }

    #endregion create category
}