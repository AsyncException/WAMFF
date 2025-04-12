using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using WAMFF.Core.Services;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace WAMFF.Core.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IFileService f_FileService = Ioc.Default.GetRequiredService<IFileService>();

    public void CanDrop(object sender, DragEventArgs e) {
        if (e.DataView.Contains(StandardDataFormats.StorageItems)) {
            IReadOnlyList<IStorageItem> items = e.DataView.GetStorageItemsAsync().GetAwaiter().GetResult();
            if (items.All(e => e is StorageFile)) {
                e.AcceptedOperation = DataPackageOperation.Copy;
            }
        }
    }

    public async void FileDropped(object sender, DragEventArgs e) {
        if (e.DataView.Contains(StandardDataFormats.StorageItems)) {
            IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
            List<string> filePaths = items.OfType<StorageFile>().Select(f => f.Path).ToList();
            f_FileService.CopyItems(filePaths);
        }
    }
}