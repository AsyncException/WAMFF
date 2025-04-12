using LiteDB;

namespace WAMFF.Core.Models;

public class Category
{
    [BsonId]
    public Guid? Id { get; set; } = Guid.CreateVersion7();

    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#FF529F";

    public static Category Default { get; } = new() { Id = Guid.Empty, Name = "Uncatagorized" };
    public static Category All { get; } = new() { Id = null, Name = "All" };

    public Category Clone() => new() { Id = Id, Name = Name, Color = Color };

    public override string ToString() => Name;

    public Category() {
    }

    public Category(string name, string color) {
        Name = name;
        Color = color;
    }
}