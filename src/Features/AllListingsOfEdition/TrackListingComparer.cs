namespace Chroomsoft.Top2000.Features;

public class TrackListingComparer : IEqualityComparer<TrackListing>
{
    public bool Equals(TrackListing? x, TrackListing? y)
    {
        if (x is null && y is null)
        {
            return true;
        }
        else  if (x is not null && y is null)
        {
            return false;
        }
        else if (x is null && y is not null)
        {
            return false;
        }
        else
        {
            return x!.Position == y!.Position;
        }
    }

    public int GetHashCode(TrackListing obj) => obj.Position.GetHashCode();
}
