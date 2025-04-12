using LiteDB;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public interface ICategoryRepository
{
    void Create(Category category);

    void Delete(Category category);

    List<Category> GetAll();

    void Update(Category category);
}

public class CategoryRepository(ILiteDatabase database) : ICategoryRepository
{
    private readonly ILiteCollection<Category> collection = database.GetCollection<Category>();

    public List<Category> GetAll() {
        return collection.FindAll().OrderBy(e => e.Name).ToList();
    }

    public void Create(Category category) {
        collection.Insert(category);
    }

    public void Update(Category category) {
        collection.Update(category);
    }

    public void Delete(Category category) {
        collection.Delete(category.Id);
    }
}