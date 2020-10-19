using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public class ObservableList<TItem> : ObservableCollection<TItem>, INotifyCollectionChanged
    {
        private static readonly NotifyCollectionChangedEventArgs EventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

        // public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void AddRange(IEnumerable<TItem> items)
        {
            this.Clear();

            items.ToList().ForEach(Add);

            //   CollectionChanged?.Invoke(this, EventArgs);
        }
    }
}
