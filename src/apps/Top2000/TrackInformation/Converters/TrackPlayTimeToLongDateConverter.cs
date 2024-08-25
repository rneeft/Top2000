using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using System;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class TrackPlayTimeToLongDateConverter : ValueConverterBase<DateTime, string>
    {
        public override string Convert(DateTime value)
        {
            var hour = value.TimeOfDay.Hours;
            var untilHour = hour + 1;

            if (untilHour > 24)
                untilHour = 0;

            var date = value.ToString("dddd d MMMM yyyy", App.DateTimeFormatProvider);

            return $"{date} {hour}:00 - {untilHour}:00";
        }
    }
}
