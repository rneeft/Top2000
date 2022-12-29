namespace Chroomsoft.Top2000App.Converters;

public sealed class EditionPlayTimeConverter : ValueConverterBase<DateTime, string>
{
    private const string ShortFormat = "dd MMM yyyy HH:mm";
    private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

    public override string Convert(DateTime dateTime) => dateTime.ToString(ShortFormat, formatProvider);
}
