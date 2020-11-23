using Chroomsoft.Top2000.WindowsApp.Common;
using System;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public class DateTimeToTimeOnlyString : ValueConverterBase<DateTime, string>
    {
        public override string Convert(DateTime value)
        {
            var hour = value.Hour + 1;

            return $"{value.Hour}:00 - {hour}:00";
        }
    }
}
