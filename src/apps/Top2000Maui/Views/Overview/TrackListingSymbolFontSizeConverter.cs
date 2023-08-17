namespace Chroomsoft.Top2000.Apps.Views.Overview;

public class TrackListingSymbolFontSizeConverter : ValueConverterBase<TrackListing, int>
{
    public override int Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null || !value.HasValue || value.Value == 0)
        {
            return 20;
        }

        return 11;
    }
}