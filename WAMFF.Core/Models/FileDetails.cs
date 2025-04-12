using LiteDB;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WAMFF.Core.Models;

/// <summary>
/// Represents the details of a file, including metadata and unique identifiers.
/// </summary>
public class FileDetails
{
    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file extension.
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file's creation date.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the file's last modified date.
    /// </summary>
    public DateTime LastModifiedDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Gets or sets the full path of the file.
    /// </summary>
    public string FullPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative path of the file.
    /// </summary>
    public string RelativePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Id of the file based on the CreatedDate.
    /// </summary>
    public long FileId { get; set; } = 0;

    /// <summary>
    /// Gets or sets the icon that this file uses.
    /// </summary>
    public BitmapImage? FileIcon { get; set; } = null;

    /// <summary>
    /// The size of the file in bytes
    /// </summary>
    public long SizeBytes { get; set; } = 0;

    /// <summary>
    /// Gets or sets the type of the file (e.g., "text", "image").
    /// </summary>
    public string FileType { get; set; } = string.Empty;

    public string HumanReadableSize => GetHumanReadableSize();

    /// <summary>
    /// Converts the SizeBytes property into a human-readable format (e.g., KB, MB, GB).
    /// </summary>
    /// <returns>A string representing the size in a human-readable format.</returns>
    public string GetHumanReadableSize() {
        decimal size = 0;
        string unit = units[0];

        if (SizeBytes > 0) {
            int log = (int)Math.Log(SizeBytes, 1000);
            size = SizeBytes / (decimal)Math.Pow(1000, log);
            unit = units[log];
        }

        return $"{size:0.##} {unit}";
    }

    //TODO:something with enums?
    private readonly string[] units = ["B", "KB", "MB", "GB", "TB"];
}

public class FileStats
{
    /// <summary>
    /// Gets or sets the id of the file these stats belong to
    /// </summary>
    [BsonId] public long FileId { get; set; } = 0;

    /// <summary>
    /// The name of the file the last time it was seen. This can be used if the id of the file changes
    /// </summary>
    public string PreviousFileName { get; set; } = string.Empty;

    /// <summary>
    /// The category of the file
    /// </summary>
    public Guid Category { get; set; } = Guid.Empty;

    /// <summary>
    /// A list of tags this file has
    /// </summary>
    public List<string> Tags { get; set; } = [];
}

public class CombinedFile
{
    public FileDetails Details { get; set; }
    public FileStats Stats { get; set; }
}