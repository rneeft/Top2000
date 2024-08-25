using System.Collections.Generic;

namespace Chroomsoft.Top2000.Features.TrackInformation
{
    public class ListingInformationDescendingComparer : Comparer<ListingInformation>
    {
        public override int Compare(ListingInformation? x, ListingInformation? y)
        {
            return y?.Edition ?? 0 - x?.Edition ?? 0;
        }
    }
}
