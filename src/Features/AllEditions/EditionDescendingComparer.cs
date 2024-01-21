namespace Chroomsoft.Top2000.Features.AllEditions;

public sealed class EditionDescendingComparer : Comparer<Edition>
{
    public override int Compare(Edition? x, Edition? y)
    {
        _ = x ?? throw new ArgumentNullException(nameof(x));
        _ = y ?? throw new ArgumentNullException(nameof(y));

        return y.Year - x.Year;
    }
}