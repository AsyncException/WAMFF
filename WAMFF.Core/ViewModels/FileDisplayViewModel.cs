using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiteDB;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.Services;
using WAMFF.Core.Utilities;

namespace WAMFF.Core.ViewModels;

public partial class FileDisplayViewModel(CombinedFile file) : ObservableObject
{
    private readonly IFileService f_FileService = Ioc.Default.GetRequiredService<IFileService>();
    private readonly IFileRepository f_FileRepository = Ioc.Default.GetRequiredService<IFileRepository>();

    [ObservableProperty]
    public partial CombinedFile File { get; set; } = file;

    [ObservableProperty]
    public partial Category Category { get; set; } = StrongReferenceMessenger.Default.Send(new CategoryRequestMessage(file.Stats.Category));

    [ObservableProperty]
    public partial Category SelectedCategory { get; set; } = Category.Default;

    public Visibility VSCodeFlyoutVisibility { get; set; } = ConfigurationProvider.CurrentConfig.IsVSCodeInstalled ? Visibility.Visible : Visibility.Collapsed;

    #region opening files

    public void OpenDoubleTap(object sender, DoubleTappedRoutedEventArgs e) {
        if (sender is Grid grid && grid.DataContext is FileDisplayViewModel file) { OpenFile(); }
    }

    [RelayCommand] public void OpenFile() => ProcessStarter.WithDefault(File.Details);
    [RelayCommand] public void OpenWith() => ProcessStarter.WithOpenWith(File.Details);
    [RelayCommand] public void OpenWithVsCode() => ProcessStarter.WithVsCode(File.Details, ConfigurationProvider.CurrentConfig);
    [RelayCommand] public void OpenInExplorer() => ProcessStarter.ShowInExplorer(File.Details);
    [RelayCommand] public void DeleteItem() => f_FileService.Delete(File.Details);

    #endregion opening files

    #region deleting

    public ContentDialog DeleteDialog { get; set; } = default!;

    [RelayCommand]
    public async Task ShowDeleteDialog() {
        ContentDialogResult result = await DeleteDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            f_FileService.Delete(File.Details);
        }
    }

    #endregion deleting

    #region Renaming

    public ContentDialog RenameDialog { get; set; } = default!;

    [ObservableProperty]
    public partial string RenameText { get; set; } = string.Empty;

    [RelayCommand]
    public async Task ShowRenameDialog() {
        RenameText = File.Details.Name;

        ContentDialogResult result = await RenameDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            if (Path.GetInvalidFileNameChars().Any(c => RenameText.Contains(c))) {
                //TODO show error message.
                return;
            }

            f_FileService.Rename(File.Details, RenameText);
        }
    }

    #endregion Renaming

    #region Changing category

    public ContentDialog CategoryDialog { get; set; } = default!;
    public List<Category> Categories { get; set; } = [];

    [RelayCommand]
    public async Task ShowCategoryDialog() {
        Categories = StrongReferenceMessenger.Default.Send(new CategoriesRequestMessage());
        ContentDialogResult result = await CategoryDialog.ShowAsync();
        if (result is ContentDialogResult.Primary && SelectedCategory is not null && SelectedCategory.Id != File.Stats.Category) {
            ILiteCollection<FileStats> collection = Ioc.Default.GetRequiredService<ILiteDatabase>().GetCollection<FileStats>();
            File.Stats.Category = SelectedCategory.Id!.Value;
            collection.Update(File.Stats);
            Category = StrongReferenceMessenger.Default.Send(new CategoryRequestMessage(File.Stats.Category));
        }
        else {
            SelectedCategory = null!;
            Categories = [];
        }
    }

    public void UpdateSuggestionBox(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            sender.ItemsSource = Categories
                .Where(e => e.Id is not null && e.Name.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }

    public void CategorySuggestionQuery(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        if(args.ChosenSuggestion is not null) {
            SelectedCategory = ((Category)args.ChosenSuggestion);
            sender.Text = SelectedCategory.Name;
        }
    }

    #endregion Changing category

    #region Changing tags
    public ContentDialog TagsDialog { get; set; } = default!;

    [ObservableProperty] public partial ObservableCollection<TagsUpdateContext> AssignedTags { get; set; } = [];
    [ObservableProperty] public partial string TagSuggestionBoxText { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = [];

    [RelayCommand]
    public async Task ShowTagsDialog() {
        Tags = StrongReferenceMessenger.Default.Send(new TagsRequestMessage());
        AssignedTags = [];
        AssignedTags.AddRange(File.Stats.Tags.Select(e => new TagsUpdateContext { Tag = e, Tags = AssignedTags }));

        ContentDialogResult result = await TagsDialog.ShowAsync();
        if (result is ContentDialogResult.Primary) {
            File.Stats.Tags = AssignedTags.Select(e => e.Tag).ToList();
            f_FileRepository.Update(File.Stats);
            StrongReferenceMessenger.Default.Send(new ForcedTagUpdateMessage());
            StrongReferenceMessenger.Default.Send(new ForcedFileUpdateMessage());
        }
        else {
            Tags = [];
            AssignedTags = [];
        }
    }

    public void TagUpdateSuggestionBox(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
            sender.ItemsSource = Tags
                .Where(e => e.Contains(sender.Text, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
    }

    public void TagSuggestionQuery(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
        string suggestion = args.ChosenSuggestion != null ? (string)args.ChosenSuggestion : args.QueryText;

        AssignedTags.Add(new TagsUpdateContext { Tag = suggestion, Tags = AssignedTags });
        AssignedTags.Replace(AssignedTags.OrderBy(e => e.Tag).ToList());

        sender.Text = "";
    }

    #endregion
}

public partial class TagsUpdateContext : ObservableObject {
    [ObservableProperty]
    public partial string Tag { get; set; } = string.Empty;
    public ObservableCollection<TagsUpdateContext> Tags { get; set; } = new();

    [RelayCommand]
    public void RemoveTag(string tag) {
        Tags.Remove(this);
    }
}