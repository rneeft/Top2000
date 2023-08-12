namespace Chroomsoft.Top2000.Features.Searching;

public static class GroupBy
{
    private static readonly IGroup defaultGroup = new GroupByNothing();

    public static IGroup Default => defaultGroup;
}

public interface IGroup
{
    IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks);
}