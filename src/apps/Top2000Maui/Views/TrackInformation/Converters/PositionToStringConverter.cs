namespace Chroomsoft.Top2000.Apps.Views.TrackInformation.Converters;

public class PositionToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? value)
    {
        return value.HasValue
            ? value.Value.ToString(App.NumberFormatProvider)
            : "-";
    }
}