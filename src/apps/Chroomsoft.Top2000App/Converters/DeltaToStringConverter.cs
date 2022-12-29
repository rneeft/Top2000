namespace Chroomsoft.Top2000App.Converters;

public sealed class DeltaToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? offset)
    {
        return offset.HasValue && offset.Value != 0
            ? PositiveInteger(offset.Value).ToString(App.NumberFormatProvider)
            : string.Empty;
    }

    private static int PositiveInteger(int value)
    {
        return value < 0
            ? value * -1
            : value;
    }
}
