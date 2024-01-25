using System.Collections.Generic;
using System.Linq;

namespace Chroomsoft.Top2000.Features.Searching
{
    public class GroupByNothing : IGroup
    {
        public IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks)
        {
            return tracks.GroupBy(x => "" + tracks.Count());
        }
    }
}
