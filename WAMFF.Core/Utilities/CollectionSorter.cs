using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.ViewModels;

namespace WAMFF.Core.Utilities;
public partial class CollectionSorter : ObservableObject
{
    private const string SORT_ASC = "\uE74A";
    private const string SORT_DESC = "\uE74B";
    private const string NO_SORT = "\uE8CB";

    private string f_CurrentSelected = nameof(NameIcon);
    [ObservableProperty] public partial string NameIcon { get; set; } = SORT_ASC;
    [RelayCommand] public  void SortByName() {
        if (f_CurrentSelected.Equals(nameof(NameIcon))) {
            Direction = Direction.Flip();
            NameIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(NameIcon);
            Direction = SortDirection.Ascending;
            NameIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string CategoryIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByCategory() {
        if (f_CurrentSelected.Equals(nameof(CategoryIcon))) {
            Direction = Direction.Flip();
            CategoryIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(CategoryIcon);
            Direction = SortDirection.Ascending;
            CategoryIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string TagsIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByTags() {
        if (f_CurrentSelected.Equals(nameof(TagsIcon))) {
            Direction = Direction.Flip();
            TagsIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(TagsIcon);
            Direction = SortDirection.Ascending;
            TagsIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string TypeIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByType() {
        if (f_CurrentSelected.Equals(nameof(TypeIcon))) {
            Direction = Direction.Flip();
            TypeIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(TypeIcon);
            Direction = SortDirection.Ascending;
            TypeIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string ExtensionIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByExtension() {
        if (f_CurrentSelected.Equals(nameof(ExtensionIcon))) {
            Direction = Direction.Flip();
            ExtensionIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(ExtensionIcon);
            Direction = SortDirection.Ascending;
            ExtensionIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string CreatedDateIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByCreatedDate() {
        if (f_CurrentSelected.Equals(nameof(CreatedDateIcon))) {
            Direction = Direction.Flip();
            CreatedDateIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(CreatedDateIcon);
            Direction = SortDirection.Ascending;
            CreatedDateIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string LastModifiedDateIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortByLastModifiedDate() {
        if (f_CurrentSelected.Equals(nameof(LastModifiedDateIcon))) {
            Direction = Direction.Flip();
            LastModifiedDateIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(LastModifiedDateIcon);
            Direction = SortDirection.Ascending;
            LastModifiedDateIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    [ObservableProperty] public partial string SizeIcon { get; set; } = NO_SORT;
    [RelayCommand] public void SortBySize() {
        if (f_CurrentSelected.Equals(nameof(SizeIcon))) {
            Direction = Direction.Flip();
            SizeIcon = Direction == SortDirection.Ascending ? SORT_ASC : SORT_DESC;
        }
        else {
            (NameIcon, CategoryIcon, TagsIcon, TypeIcon, ExtensionIcon, CreatedDateIcon, LastModifiedDateIcon, SizeIcon) = (NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT, NO_SORT);
            f_CurrentSelected = nameof(SizeIcon);
            Direction = SortDirection.Ascending;
            SizeIcon = SORT_ASC;
        }

        StrongReferenceMessenger.Default.Send(new SortChangedMessage(true));
    }

    public SortDirection Direction { get; set; } = SortDirection.Ascending;

    public IEnumerable<FileDisplayViewModel> SortList(IEnumerable<FileDisplayViewModel> input) => f_CurrentSelected switch {
        nameof(NameIcon) => input.OrderByDirection(Direction, e => e.File.Details.Name),
        nameof(CategoryIcon) => input.OrderByDirection(Direction, e => e.Category.Name),
        nameof(TagsIcon) => input.OrderByDirection(Direction.Flip(), e => e.File.Stats.Tags.Count).ThenBy(e => e.File.Details.Name),
        nameof(TypeIcon) => input.OrderByDirection(Direction, e => e.File.Details.FileType).ThenBy(e => e.File.Details.Name),
        nameof(ExtensionIcon) => input.OrderByDirection(Direction, e => e.File.Details.Extension).ThenBy(e => e.File.Details.Name),
        nameof(CreatedDateIcon) => input.OrderByDirection(Direction.Flip(), e => e.File.Details.CreatedDate).ThenBy(e => e.File.Details.Name),
        nameof(LastModifiedDateIcon) => input.OrderByDirection(Direction.Flip(), e => e.File.Details.LastModifiedDate).ThenBy(e => e.File.Details.Name),
        nameof(SizeIcon) => input.OrderByDirection(Direction.Flip(), e => e.File.Details.SizeBytes).ThenBy(e => e.File.Details.Name),
        _ => throw new NotImplementedException(),
    };
}

public static class CollectionSorterExtensions {
    public static IOrderedEnumerable<TSource> OrderByDirection<TSource, TKey>(this IEnumerable<TSource> source, SortDirection direction, Func<TSource, TKey> keySelector) {
        return direction == SortDirection.Descending
            ? source.OrderByDescending(keySelector)
            : source.OrderBy(keySelector);
    }

    public static SortDirection Flip(this SortDirection direction) => direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
}

public enum SortDirection {
    Ascending,
    Descending
}