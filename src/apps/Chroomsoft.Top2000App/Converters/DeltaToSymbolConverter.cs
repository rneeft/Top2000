namespace Chroomsoft.Top2000App.Converters;

public sealed class DeltaToSymbolConverter : ValueConverterBase<TrackListing, string>
{
    public override string Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null)
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
