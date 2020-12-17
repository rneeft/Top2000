using Chroomsoft.Top2000.Apps.Common;
using System;
using System.Globalization;

namespace Chroomsoft.Top2000.Apps.Overview.Date
{
    public class DateTimeKeyToString : ValueConverterBase<DateTime, string>
    {
        private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

        public override string Convert(DateTime value)
        {
            var hour = value.Hour + 1;
            var date = value.ToString("dddd dd MMM H", formatProvider);

            return $"{date}:00 - {hour}:00";
        }
    }
}
