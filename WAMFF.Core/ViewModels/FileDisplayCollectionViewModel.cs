using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.Utilities;

namespace WAMFF.Core.ViewModels;

public partial class FileDisplayCollectionViewModel : ObservableObject
{
    private string f_SearchQuery = string.Empty;
    private Category? f_SelectedCategory = Category.All;
    private List<string> f_SelectedTags = [];
    private List<FileDisplayViewModel> f_BackingFiles = StrongReferenceMessenger.Default.Send(new FilesRequestMessage()).Response.Select(e => new FileDisplayViewModel(e)).ToList();

    public ObservableCollection<FileDisplayViewModel> FileDisplayViewModels { get; set; } = [];

    public FileDisplayCollectionViewModel() {
        StrongReferenceMessenger.Default.Register<FileDisplayCollectionViewModel, FilesChangedMessage>(this, static (r, m) => {
            r.f_BackingFiles = m.Value.Select(e => new FileDisplayViewModel(e)).ToList();
            r.ApplyFilters();
        });

        StrongReferenceMessenger.Default.Register<FileDisplayCollectionViewModel, SearchQueryChangedMessage>(this, static (r, m) => {
            r.f_SearchQuery = m.Value;
            r.ApplyFilters();
        });

        StrongReferenceMessenger.Default.Register<FileDisplayCollectionViewModel, CategoryChangedMessage>(this, static (r, m) => {
            r.f_SelectedCategory = m.Value;
            r.ApplyFilters();
        });

        StrongReferenceMessenger.Default.Register<FileDisplayCollectionViewModel, TagsChangedMessage>(this, static (r, m) => {
            r.f_SelectedTags = m.Value;
            r.ApplyFilters();
        });

        ApplyFilters();
    }

    private void ApplyFilters() {
#if DEBUG
        Stopwatch stopwatch = Stopwatch.StartNew();
#endif
        IEnumerable<FileDisplayViewModel> files = f_BackingFiles;

        if (!string.IsNullOrEmpty(f_SearchQuery)) {
            files = files.Where(e =>
                e.File.Details.Name.Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Details.FileType.Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Details.Extension.Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Details.CreatedDate.ToIsoDateString().Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Details.LastModifiedDate.ToIsoDateString().Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Details.RelativePath.Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase) ||
                e.File.Stats.Tags.Any(e => e.Contains(f_SearchQuery, StringComparison.CurrentCultureIgnoreCase))
            );
        }

        if (f_SelectedCategory?.Id is not null) {
            files = files.Where(e => e.File.Stats.Category == f_SelectedCategory.Id);
        }

        if(f_SelectedTags.Count > 0) {
            files = files.Where(e => f_SelectedTags.All(t => e.File.Stats.Tags.Contains(t)));
        }


        FileDisplayViewModels.Replace(files);

#if DEBUG
        stopwatch.Stop();
#endif
    }
}