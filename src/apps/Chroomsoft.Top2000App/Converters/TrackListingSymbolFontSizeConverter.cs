namespace Chroomsoft.Top2000App.Converters;

public sealed class TrackListingSymbolFontSizeConverter : ValueConverterBase<TrackListing, double>
{
    public override double Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null || value.Value == 0)
        {
            return 20;
        }

        return 11;
    }
}
