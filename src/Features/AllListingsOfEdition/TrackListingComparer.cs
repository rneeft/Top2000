using System.Collections.Generic;

namespace Chroomsoft.Top2000.Features
{
    public class TrackListingComparer : IEqualityComparer<TrackListing>
    {
        public bool Equals(TrackListing x, TrackListing y) => x.Position == y.Position;

        public int GetHashCode(TrackListing obj) => obj.Position.GetHashCode();
    }
}
