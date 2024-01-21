namespace Chroomsoft.Top2000.Apps.Views.Overview;

public class EditionPlayTimeConverter : ValueConverterBase<DateTime, string>
{
    private const string ShortFormat = "dd MMM yyyy HH:mm";

    public override string Convert(DateTime dateTime) => dateTime.ToString(ShortFormat, App.DateTimeFormatProvider);
}