using MediatR;
using SQLite;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.AllListingsOfEdition
{
    public class AllListingsOfEditionRequest : IRequest<ImmutableHashSet<TrackListing>>
    {
        public AllListingsOfEditionRequest(int year)
        {
            Year = year;
        }

        public int Year { get; }
    }

    public class AllListingsOfEditionRequestHandler : IRequestHandler<AllListingsOfEditionRequest, ImmutableHashSet<TrackListing>>
    {
        private readonly SQLiteAsyncConnection connection;
        private readonly TrackCountHolder trackCountHolder;

        public AllListingsOfEditionRequestHandler(SQLiteAsyncConnection connection, TrackCountHolder trackCountHolder)
        {
            this.connection = connection;
            this.trackCountHolder = trackCountHolder;
        }

        public async Task<ImmutableHashSet<TrackListing>> Handle(AllListingsOfEditionRequest request, CancellationToken cancellationToken)
        {
            var counters = await trackCountHolder.CountTrackCountForEditionAsync(connection, request.Year);

            var sql =
                      "SELECT Listing.TrackId, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist " +
                      "FROM Listing JOIN Track ON Listing.TrackId = Id " +
                      "LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = ? " +
                      $"WHERE Listing.Edition = ?";

            var items = await connection.QueryAsync<TrackListing>(sql, request.Year - 1, request.Year);
            var itemWithNullDelta = items.Where(x => x.Delta is null);
            foreach (var item in itemWithNullDelta)
            {
                var inCounters = counters.Find(x => x.TrackId == item.TrackId);
                if (inCounters?.TrackCount > 1)
                {
                    item.IsRecurring = true;
                }
            }

            return  items.ToImmutableHashSet(new TrackListingComparer());
        }
    }
}
