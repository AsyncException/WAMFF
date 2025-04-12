using CommunityToolkit.Mvvm.Messaging.Messages;
using WAMFF.Core.Models;

namespace WAMFF.Core.Messages;

public class CategoriesRequestMessage : RequestMessage<List<Category>>;

public class CategoryChangedMessage(Category? value) : ValueChangedMessage<Category?>(value);

public class CategoryRequestMessage(Guid categoryId) : RequestMessage<Category>
{
    public Guid CategoryId { get; } = categoryId;
}

public class ForceCategoryRefreshMessage : RequestMessage<bool>;