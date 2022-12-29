namespace Chroomsoft.Top2000.Features.Groupings;

public sealed class GroupByHundreds : IGroup<TrackListing>
{
    private const int GroupSize = 100;

    public IEnumerable<IGrouping<string, TrackListing>> Group(IEnumerable<TrackListing> tracks)
    {
        return tracks.GroupBy(Hunderd);
    }

    private static string Hunderd(TrackListing listing)
    {
        if (listing.Position < 100) return "1 - 100";
        if (listing.Position >= 1900) return "1900 - 2000";

        var min = listing.Position / GroupSize * GroupSize;
        var max = min + GroupSize;

        return $"{min} - {max}";
    }
}
