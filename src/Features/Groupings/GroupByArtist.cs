using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Features.Groupings;

public sealed class GroupByArtist : IGroupSearch
{
    public IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks)
    {
        return tracks.GroupBy(x => x.Artist);
    }
}