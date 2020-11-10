using System.Collections.Generic;
using System.Linq;

namespace Chroomsoft.Top2000.Features.Searching
{
    public interface ISort
    {
        IOrderedEnumerable<Track> Sort(IEnumerable<Track> tracks);
    }
}
