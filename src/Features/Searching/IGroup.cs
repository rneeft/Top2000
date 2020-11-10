using System.Collections.Generic;
using System.Linq;

namespace Chroomsoft.Top2000.Features.Searching
{
    public interface IGroup
    {
        IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks);
    }
}
