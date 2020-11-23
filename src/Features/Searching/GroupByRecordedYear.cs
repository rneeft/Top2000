using System.Collections.Generic;
using System.Linq;

namespace Chroomsoft.Top2000.Features.Searching
{
    public class GroupByRecordedYear : IGroup
    {
        public IEnumerable<IGrouping<string, Track>> Group(IEnumerable<Track> tracks)
            => tracks.GroupBy(x => "" + x.RecordedYear);
    }
}
