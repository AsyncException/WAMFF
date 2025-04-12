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
        foreach(FileStats stats in collection.Find(e => e.Category == id)) {
            stats.Category = Guid.Empty;
            collection.Update(stats);
        }
    }

    public void Update(FileStats stats) {
        collection.Update(stats);
    }
}