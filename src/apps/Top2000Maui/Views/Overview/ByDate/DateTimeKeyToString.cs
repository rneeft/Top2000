﻿namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate;

public class DateTimeKeyToString : ValueConverterBase<DateTime, string>
{
    public override string Convert(DateTime value)
    {
        var hour = value.Hour + 1;
        var date = value.ToString("dddd dd MMM H", App.DateTimeFormatProvider);

        return $"{date}:00 - {hour}:00";
    }
}