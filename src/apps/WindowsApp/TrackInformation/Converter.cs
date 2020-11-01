using Chroomsoft.Top2000.Features.TrackInformation;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public static class Converter
    {
        private static readonly SolidColorBrush YellowColour = new SolidColorBrush(Color.FromArgb(255, 255, 192, 0));
        private static readonly SolidColorBrush RedColour = new SolidColorBrush(Color.FromArgb(255, 221, 48, 57));
        private static readonly SolidColorBrush GreenColour = new SolidColorBrush(Color.FromArgb(255, 112, 173, 71));
        private static readonly SolidColorBrush GreyColour = new SolidColorBrush(Color.FromArgb(255, 103, 103, 103));

        private static readonly Thickness NoMargin = new Thickness(0, 0, 0, 0);
        private static readonly Thickness DecreasedMargin = new Thickness(0, 1, 0, 0);

        public static string ToSymbol(ListingStatus status) => status switch
        {
            ListingStatus.New => "\xE7C1",
            ListingStatus.Decreased => "\xE96E",
            ListingStatus.Increased => "\xE96D",
            ListingStatus.Unchanged => "\xE94E",
            ListingStatus.Back => "\xE248",
            _ => "\xE949",
        };

        public static SolidColorBrush ToSymbolColour(ListingStatus status) => status switch
        {
            ListingStatus.New => YellowColour,
            ListingStatus.Back => YellowColour,
            ListingStatus.Decreased => RedColour,
            ListingStatus.Increased => GreenColour,
            _ => GreyColour,
        };

        public static Thickness ToMargin(ListingStatus status) => status switch
        {
            ListingStatus.Decreased => DecreasedMargin,
            _ => NoMargin,
        };

        public static string MakePositive(int? value)
        {
            if (value is null || value == 0) return string.Empty;

            return (value < 0)
                    ? (value * -1).ToString()
                    : value.ToString();
        }

        public static string PositionString(int? value)
        {
            return value is null ? "-" : value.ToString();
        }

        public static string MakeDateTimeString(DateTime? value)
        {
            if (value is null) return string.Empty;

            var hour = value.Value.Hour;
            var untilHour = hour + 1;
            if (untilHour > 24)
                untilHour = 0;

            var date = value.Value.ToString("dddd d MMMM yyyy");

            return $"{date} {hour}:00 - {untilHour}:00";
        }

        public static Visibility HideWhenNotListed(ListingStatus status)
        {
            return status == ListingStatus.NotListed
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}
