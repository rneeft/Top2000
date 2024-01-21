namespace Chroomsoft.Top2000.Apps.Views.TrackInformation.Converters;

public class BooleanInvertedConverter : ValueConverterBase<bool, bool>
{
    public override bool Convert(bool value) => !value;
}