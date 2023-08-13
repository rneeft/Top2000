using Chroomsoft.Top2000.Features.AllListingsOfEdition;

namespace Chroomsoft.Top2000.Apps.Overview;

public class DeltaToSymbolConverter : ValueConverterBase<TrackListing, string>
{
    public override string Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null || !value.HasValue)
        {
            return Symbols.New;
        }

        if (value.Value > 0)
        {
            return Symbols.Up;
        }

        if (value.Value < 0)
        {
            return Symbols.Down;
        }

        return Symbols.Same;
    }
}