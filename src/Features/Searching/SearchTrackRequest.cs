using MediatR;
using SQLite;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.Searching
{
    public class SearchTrackRequest : IRequest<ImmutableHashSet<Track>>
    {
        public SearchTrackRequest(string queryString)
        {
            QueryString = queryString;
        }

        public string QueryString { get; }
    }

    public class SearchTrackRequestHandler : IRequestHandler<SearchTrackRequest, ImmutableHashSet<Track>>
    {
        private readonly SQLiteAsyncConnection connection;

        public SearchTrackRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ImmutableHashSet<Track>> Handle(SearchTrackRequest request, CancellationToken cancellationToken)
        {
            if (int.TryParse(request.QueryString, out int year))
            {
                var sql =
               "SELECT Id, Title, Artist, RecordedYear " +
               "FROM Track " +
               "WHERE RecordedYear = ?";

                return (await connection.QueryAsync<Track>(sql, year).ConfigureAwait(false))
                    .ToImmutableHashSet(new TrackComparer());
            }
            else
            {
                var sql =
              "SELECT Id, Title, Artist, RecordedYear " +
              "FROM Track " +
              "WHERE (Title LIKE ?) OR (Artist LIKE ?)";

                return (await connection.QueryAsync<Track>(sql, $"%{request.QueryString}%", $"%{request.QueryString}%").ConfigureAwait(false))
                    .ToImmutableHashSet(new TrackComparer());
            }
        }
    }

    public class TrackComparer : IEqualityComparer<Track>
    {
        public bool Equals(Track x, Track y) => x.Id == y.Id;

        public int GetHashCode(Track obj) => obj.Id.GetHashCode();
    }
}
