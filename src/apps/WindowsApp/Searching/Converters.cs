#nullable enable

using Chroomsoft.Top2000.Features.Searching;
using Windows.UI.Xaml;

namespace Chroomsoft.Top2000.WindowsApp.Searching
{
    public static class Converters
    {
        public static Visibility ShowWhenGrouped(GroupViewModel group)
        {
            return group.Value is GroupByNothing
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public static Visibility HideWhenGrouped(GroupViewModel group)
        {
            return group.Value is GroupByNothing
                            ? Visibility.Visible
                            : Visibility.Collapsed;
        }
    }
}
