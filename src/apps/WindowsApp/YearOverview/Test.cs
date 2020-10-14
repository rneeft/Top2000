using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public class Test : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
