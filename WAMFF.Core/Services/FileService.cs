using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using LiteDB;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;
using WAMFF.Core.Utilities;
using Windows.ApplicationModel.VoiceCommands;

namespace WAMFF.Core.Services;

public interface IFileService
{
    List<CombinedFile> GetFiles();

    void Delete(FileDetails file);

    void Rename(FileDetails file, string new_name);

    void CopyItems(List<string> files);

    void AttachDispatcherQueue(DispatcherQueue dispatcherQueue);
    void StartCleanUp();
}

public class FileService : IFileService
{
    private readonly ILiteDatabase database;
    private string[] paths = [];
    private FileSystemWatcher[] watchers = [];
    private readonly Dictionary<string, Utilities.FileAttributes> attribute_store = [];
    private DispatcherQueue DispatcherQueue = null!;

    public FileService(ILiteDatabase database) {
        this.database = database;
        BuildWatchers();

        StrongReferenceMessenger.Default.Register<FileService, FilesRequestMessage>(this, static async (r, m) => {
            await r.DispatcherQueue.EnqueueAsync(() => {
                m.Reply(r.GetFiles());
            });
        });

        StrongReferenceMessenger.Default.Register<FileService, ForcedFileUpdateMessage>(this, static (r, m) => {
            r.Invoke(null, null!);
            m.Reply(true);
        });
    }

    public void AttachDispatcherQueue(DispatcherQueue dispatcherQueue) {
        DispatcherQueue = dispatcherQueue;
    }

    private void BuildWatchers() {
        paths = ConfigurationProvider.CurrentConfig.DirectoryPath.ToArray();
        watchers = new FileSystemWatcher[paths.Length];

        for (int i = 0; i < paths.Length; i++) {
            string path = paths[i];
            if (!Path.Exists(path))
                Directory.CreateDirectory(path);

            FileSystemWatcher watcher = new(path) { EnableRaisingEvents = true };

            watcher.Changed += Invoke;
            watcher.Created += Invoke;
            watcher.Deleted += Invoke;
            watcher.Renamed += Invoke;

            watchers[i] = watcher;
        }
    }

    private async void Invoke(object? sender, FileSystemEventArgs args) {
        await DispatcherQueue.EnqueueAsync(() => {
            StrongReferenceMessenger.Default.Send(new FilesChangedMessage(GetFiles()));
        });
    }

    public List<CombinedFile> GetFiles() {
        ILiteCollection<FileStats> collection = database.GetCollection<FileStats>();

        List<CombinedFile> files = [];
        foreach (string root in paths) {
            foreach (string full_path in Directory.GetFiles(root, "*", SearchOption.AllDirectories)) {
                try {
                    FileInfo info = new(full_path);
                    long file_id = GetFileId(info);
                    string backup_name = info.Name[..^info.Extension.Length];
                    Utilities.FileAttributes attrb = GetFileAttributes(info.FullName, info.Extension);

                    CombinedFile combined_file = new() {
                        Details = new FileDetails {
                            Name = backup_name,
                            Extension = info.Extension,
                            CreatedDate = info.CreationTime,
                            LastModifiedDate = info.LastWriteTime,
                            FullPath = info.FullName,
                            RelativePath = Path.GetRelativePath(root, info.DirectoryName ?? string.Empty).Trim('.'),
                            FileId = file_id,
                            SizeBytes = info.Length,
                            FileIcon = attrb.Icon,
                            FileType = attrb.Type
                        },
                        Stats = GetOrCreateStats(file_id, backup_name, collection)
                    };

                    files.Add(combined_file);
                }
                catch(FileNotFoundException) {
                    //This can occur when a user deletes items in bulk and a scan is executed while the deletion is still in progress.
                    //In this case the exception can be ignored without issue.
                }
            }
        }

        return files;
    }

    private static FileStats GetOrCreateStats(long file_id, string backup_name, ILiteCollection<FileStats> collection) {
        FileStats? stats = collection.FindById(file_id) ?? collection.FindOne(e => e.PreviousFileName == backup_name);

        if (stats is null) {
            stats = new FileStats { FileId = file_id, PreviousFileName = backup_name };
            collection.Insert(stats);
        }

        //Sync up file names so if the id changes the name will be updated
        if (stats.PreviousFileName != backup_name) {
            stats.PreviousFileName = backup_name;
            collection.Update(stats);
        }

        return stats;
    }

    private static long GetFileId(FileInfo file) {
        return file.CreationTimeUtc.ToFileTime();
    }

    private Utilities.FileAttributes GetFileAttributes(string fileName, string extension) {
        //Get The icon if it doesnt exist in the store yet
        if (!attribute_store.TryGetValue(extension, out Utilities.FileAttributes? attrb)) {
            attrb = FileTools.GetIconForExtension(fileName);
            attribute_store.Add(extension, attrb);
        }

        return attrb;
    }

    public void Delete(FileDetails file) {
        File.Delete(file.FullPath);
    }

    public void Rename(FileDetails file, string new_name) {
        string new_path = Path.Combine(Path.GetDirectoryName(file.FullPath)!, string.Concat(new_name, file.Extension));
        File.Move(file.FullPath, new_path);
        file.FullPath = new_path;
    }

    public void CopyItems(List<string> files) {
        foreach (string file in files) {
            string new_path = Path.Combine(ConfigurationProvider.CurrentConfig.DirectoryPath[0], Path.GetFileName(file));
            File.Copy(file, new_path, true);
        }
    }

    public void StartCleanUp() {
        ILiteCollection<FileStats> collection = database.GetCollection<FileStats>();
        List<FileStats> stats = collection.FindAll().ToList();
        List<(long, string)> existing = [];
        foreach (string root in paths) {
            foreach (string full_path in Directory.GetFiles(root, "*", SearchOption.AllDirectories)) {
                FileInfo info = new(full_path);
                long file_id = GetFileId(info);
                string backup_name = info.Name[..^info.Extension.Length];

                existing.Add((file_id, backup_name));
            }
        }

        List<long> should_delete = stats.Where(e => !existing.Any(c => e.FileId == c.Item1 || e.PreviousFileName == c.Item2)).Select(e => e.FileId).ToList();
        collection.DeleteMany(e => should_delete.Contains(e.FileId));
    }
}