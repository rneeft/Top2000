namespace Chroomsoft.Top2000.Apps.Views.Overview;

public sealed class DeltaToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? offset)
    {
        return offset.HasValue && offset.Value != 0
            ? Math.Abs(offset.Value).ToString(App.NumberFormatProvider)
            : string.Empty;
    }
}