namespace Chroomsoft.Top2000.Apps.Views.TrackInformation.Converters;

public class OffsetToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? offset)
    {
        return offset.HasValue && offset.Value != 0
            ? Math.Abs(offset.Value).ToString(App.NumberFormatProvider)
            : string.Empty;
    }
}