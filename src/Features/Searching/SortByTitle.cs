namespace Chroomsoft.Top2000.Features.Searching;

public sealed class SortByTitle : ISort
{
    public IOrderedEnumerable<Track> Sort(IEnumerable<Track> tracks)
        => tracks.OrderBy(x => x.Title);
}