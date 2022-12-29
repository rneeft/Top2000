namespace Chroomsoft.Top2000.Features.Sortings;

public sealed class SortByTitle : ISortSearch
{
    public IOrderedEnumerable<Track> Sort(IEnumerable<Track> tracks)
    {
        return tracks.OrderBy(x => x.Title);
    }
}