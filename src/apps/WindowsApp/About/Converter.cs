#nullable enable

using Windows.UI.Xaml;

namespace Chroomsoft.Top2000.WindowsApp.About
{
    public static class Converter
    {
        public static bool Not(bool value) => !value;

        public static Visibility VisibilityIfTrue(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
    }
}
