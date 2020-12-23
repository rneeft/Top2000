using System.Linq;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public class ObservableGroupedList<TKey, TItem> : ObservableList<IGrouping<TKey, TItem>> { }
}
