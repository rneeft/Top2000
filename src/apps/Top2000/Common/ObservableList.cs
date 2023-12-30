using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Chroomsoft.Top2000.Apps.Common
{
    public class ObservableList<TItem> : List<TItem>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// Removed all the items in the list, add the list of items and notify the observers.
        /// </summary>
        /// <param name="items">Items to add</param>
        public void ClearAddRange(IEnumerable<TItem> items)
        {
            this.Clear();
            items.ToList().ForEach(Add);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
