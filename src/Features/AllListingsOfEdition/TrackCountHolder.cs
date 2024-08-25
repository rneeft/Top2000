using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.AllListingsOfEdition
{
    public class TrackCountHolder
    {
        Dictionary<int, List<TrackCounter>> counters = new Dictionary<int, List<TrackCounter>>();

        public async Task<List<TrackCounter>> CountTrackCountForEditionAsync(SQLiteAsyncConnection connection, int edition)
        {
            if (!counters.ContainsKey(edition))
            {
                var sql = "SELECT TrackId, COUNT(TrackId) AS TrackCount " +
                    "FROM Listing " +
                    "WHERE Edition <= ? " +
                    "GROUP BY TrackId";

                var trackCounters = await connection.QueryAsync<TrackCounter>(sql, edition);
                var list = trackCounters.Where(x => x.TrackCount > 1).ToList();
                counters.Add(edition, list);
            }

            return counters[edition];
        }

        
    }
}
