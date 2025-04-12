using CommunityToolkit.Mvvm.Messaging;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAMFF.Core.Messages;
using WAMFF.Core.Models;

namespace WAMFF.Core.Services;

public interface ITagsRepository
{
    List<string> GetTags();
}

public class TagsRespository : ITagsRepository
{
    private readonly ILiteCollection<FileStats> collection;

    public List<string> GetTags() => collection.FindAll()
         .SelectMany(x => x.Tags)
         .Distinct()
         .OrderBy(x => x)
         .ToList();


    public TagsRespository(ILiteDatabase database) {
        collection = database.GetCollection<FileStats>();

        StrongReferenceMessenger.Default.Register<TagsRespository, TagsRequestMessage>(this, static (r, m) => {
            m.Reply(r.GetTags());
        });
    }
}
