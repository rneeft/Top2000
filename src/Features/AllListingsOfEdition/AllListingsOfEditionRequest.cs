using MediatR;
using SQLite;
using System.Collections.Immutable;
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

        public AllListingsOfEditionRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ImmutableHashSet<TrackListing>> Handle(AllListingsOfEditionRequest request, CancellationToken cancellationToken)
        {
            var sql =
                      "SELECT Listing.TrackId, Listing.Position, (p.Position - Listing.Position) AS Delta, Listing.PlayUtcDateAndTime, Title, Artist " +
                      "FROM Listing JOIN Track ON Listing.TrackId = Id " +
                      "LEFT JOIN Listing as p ON p.TrackId = Id AND p.Edition = ? " +
                      $"WHERE Listing.Edition = ?";

            return (await connection.QueryAsync<TrackListing>(sql, request.Year - 1, request.Year).ConfigureAwait(false))
                .ToImmutableHashSet(new TrackListingComparer());
        }
    }
}
