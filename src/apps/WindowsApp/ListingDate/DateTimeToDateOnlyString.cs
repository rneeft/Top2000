using Chroomsoft.Top2000.WindowsApp.Common;
using System;
using System.Globalization;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public class DateTimeToDateOnlyString : ValueConverterBase<DateTime, string>
    {
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public override string Convert(DateTime value)
        {
            return value.ToString("dddd dd MMMM", formatProvider);
        }
    }
}
