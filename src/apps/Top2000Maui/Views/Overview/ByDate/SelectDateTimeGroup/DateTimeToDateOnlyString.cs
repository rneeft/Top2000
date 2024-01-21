namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate.SelectDateTimeGroup;

public class DateTimeToDateOnlyString : ValueConverterBase<DateTime, string>
{
    public override string Convert(DateTime value)
    {
        return value.ToString("dddd dd MMMM", App.DateTimeFormatProvider);
    }
}