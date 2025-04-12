using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using WAMFF.Core.Messages;

namespace WAMFF.Core.ViewModels;

public partial class TitleBarViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string SearchQuery { get; set; } = string.Empty;

    private CancellationTokenSource _cts = new();

    partial void OnSearchQueryChanged(string value) {
        //If the search value is cleared, immidiately apply the filters and make sure the other searches are cancelled
        if (string.IsNullOrEmpty(value)) {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new();
            StrongReferenceMessenger.Default.Send(new SearchQueryChangedMessage(SearchQuery));
        }
        else {
            DebounceSearch();
        }
    }

    private async void DebounceSearch() {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new();
        try {
            await Task.Delay(500, _cts.Token);
            StrongReferenceMessenger.Default.Send(new SearchQueryChangedMessage(SearchQuery));
        }
        catch (TaskCanceledException) { }
    }
}