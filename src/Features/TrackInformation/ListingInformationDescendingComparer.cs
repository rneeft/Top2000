namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class ListingInformationDescendingComparer : Comparer<ListingInformation>
{
    public override int Compare(ListingInformation? x, ListingInformation? y)
    {
        if (x is null || y is null)
        {
            return -1;
        }

        return y.Edition - x.Edition;
    }
}