using System;
using System.Globalization;
using Windows.UI.Xaml;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public static class Converters
    {
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public static string ToShortLocalTime(DateTime dateTime) => dateTime.ToLocalTime().ToString("dd MMM yyyy HH:mm", formatProvider);

        public static Visibility ShowWhenTrue(bool value) => value ? Visibility.Visible : Visibility.Collapsed;
    }
}
