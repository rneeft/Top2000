using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Features.Groupings;

public static class GroupBy
{
    private static readonly IGroupSearch defaultGroup = new GroupByNothing();

    public static IGroupSearch Default => defaultGroup;
}

public interface IGroup<T>
{
    IEnumerable<IGrouping<string, T>> Group(IEnumerable<T> tracks);
}

public interface IGroupSearch : IGroup<Track> { }