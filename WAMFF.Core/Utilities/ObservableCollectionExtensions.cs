using System.Collections.ObjectModel;

namespace WAMFF.Core.Utilities;

public static class ObservableCollectionExtensions
{
    public static void Replace<T>(this ObservableCollection<T> collection, IEnumerable<T> items) {
        collection.Clear();
        collection.AddRange(items);
    }

    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items) {
        foreach (T item in items) {
            collection.Add(item);
        }
    }
}