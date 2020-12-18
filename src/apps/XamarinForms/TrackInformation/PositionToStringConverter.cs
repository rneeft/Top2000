using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Apps.XamarinForms;
using System;

namespace Chroomsoft.Top2000.Apps.TrackInformation
{
    public class OffsetToStringConverter : ValueConverterBase<int, string>
    {
        public override string Convert(int value)
        {
            if (value == int.MaxValue || value == 0)
                return string.Empty;

            if (value < 0)
                return $"{value * -1}";

            return value.ToString(App.NumberFormatProvider);
        }
    }

    public class PositionToStringConverter : ValueConverterBase<int?, string>
    {
        public override string Convert(int? value)
        {
            return value.HasValue
                ? value.Value.ToString(App.NumberFormatProvider)
                : "-";
        }
    }

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
