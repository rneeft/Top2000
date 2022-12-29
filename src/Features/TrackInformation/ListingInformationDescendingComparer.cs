namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class ListingInformationDescendingComparer : Comparer<Listing>
{
    public override int Compare(Listing? x, Listing? y)
    {
        if (x is null || y is null)
        {
            return 0;
        }

        return y.Edition - x.Edition;
    }
}
