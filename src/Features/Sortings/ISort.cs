namespace Chroomsoft.Top2000.Features.Sortings;

public static class SortBy
{
    private static readonly ISortSearch defaultSort = new SortByTitle();

    public static ISortSearch Default => defaultSort;
}

public interface ISort<T>
{
    IOrderedEnumerable<T> Sort(IEnumerable<T> tracks);
}

public interface ISortSearch : ISort<Track> { }