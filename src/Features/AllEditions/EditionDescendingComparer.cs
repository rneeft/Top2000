using System.Collections.Generic;

namespace Chroomsoft.Top2000.Features.AllEditions
{
    public class EditionDescendingComparer : Comparer<Edition>
    {
        public override int Compare(Edition x, Edition y)
        {
            return y.Year - x.Year;
        }
    }
}
