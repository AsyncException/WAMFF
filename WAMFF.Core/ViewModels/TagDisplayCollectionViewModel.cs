using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAMFF.Core.Messages;
using WAMFF.Core.Services;
using WAMFF.Core.Utilities;

namespace WAMFF.Core.ViewModels;

public partial class TagDisplayCollectionViewModel : ObservableObject
{
    private readonly ITagsRepository f_TagsRepository = Ioc.Default.GetRequiredService<ITagsRepository>();
    
    public ObservableCollection<string> Tags { get; set; } = [];
    public IList<string> SelectedTags { get; set; } = [];

    public TagDisplayCollectionViewModel() {
        StrongReferenceMessenger.Default.Register<TagDisplayCollectionViewModel, ForcedTagUpdateMessage>(this, (r, m) => {
            r.Tags.Replace(r.f_TagsRepository.GetTags());
            m.Reply(true);
        });

        Tags.Replace(f_TagsRepository.GetTags());
    }

    public void UpdateSelection(object sender, SelectionChangedEventArgs e) {
        if (sender is ListView listView) {
            List<string> selection = listView.SelectedItems.Cast<string>().ToList();
            SelectedTags = selection;
            StrongReferenceMessenger.Default.Send(new TagsChangedMessage(selection));
        }
    }
}
