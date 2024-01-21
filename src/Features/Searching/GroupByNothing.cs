namespace Chroomsoft.Top2000.Features.Searching;

public sealed class GroupByNothing : IGroup
{
    public IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks)
    {
        return tracks.GroupBy(x => string.Empty);
    }
}