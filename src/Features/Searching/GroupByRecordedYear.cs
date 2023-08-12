namespace Chroomsoft.Top2000.Features.Searching;

public sealed class GroupByRecordedYear : IGroup
{
    public IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks)
        => tracks.GroupBy(x => x.RecordedYear.ToString());
}