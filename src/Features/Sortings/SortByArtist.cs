using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Features.Sortings;

public sealed class SortByArtist : ISortSearch
{
    public IOrderedEnumerable<Track> Sort(IEnumerable<Track> tracks)
    {
        return tracks.OrderBy(x => x.Artist);
    }
}