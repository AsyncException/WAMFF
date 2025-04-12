using LiteDB;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public interface IFileRepository
{
    void CleanDeletedCategory(Guid id);
    void Update(FileStats stats);
}

public class FileRepository(ILiteDatabase database) : IFileRepository
{
    private readonly ILiteCollection<FileStats> collection = database.GetCollection<FileStats>();

    public void CleanDeletedCategory(Guid id) {
        collection.UpdateMany(
            e => new FileStats { Category = Guid.Empty, FileId = e.FileId, PreviousFileName = e.PreviousFileName, Tags = e.Tags },
            e => e.Category == id
            );
    }

    public void Update(FileStats stats) {
        collection.Update(stats);
    }
}