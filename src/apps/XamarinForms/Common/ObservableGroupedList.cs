using System.Linq;

namespace Chroomsoft.Top2000.Apps.Common
{
    public class ObservableGroupedList<TKey, TItem> : ObservableList<IGrouping<TKey, TItem>> { }

}
