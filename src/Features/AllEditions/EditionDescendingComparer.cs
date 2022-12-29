namespace Chroomsoft.Top2000.Features;

public sealed class EditionDescendingComparer : Comparer<Edition>
{
    public override int Compare(Edition? x, Edition? y)
    {
        var result = y?.Year - x?.Year;

        return result.GetValueOrDefault(0);
    }
}