namespace Chroomsoft.Top2000.Features.Searching;

public static class SortBy
{
    private static readonly ISort defaultSort = new SortByTitle();

    public static ISort Default => defaultSort;
}

public interface ISort
{
    IOrderedEnumerable<Track> Sort(IEnumerable<Track> tracks);
}