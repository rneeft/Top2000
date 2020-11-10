using MediatR;
using SQLite;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.Searching
{
    public class SearchTrackRequest : IRequest<ReadOnlyCollection<IGrouping<string, Track>>>
    {
        public SearchTrackRequest(string queryString, ISort sorting, IGroup group)
        {
            QueryString = queryString;
            this.Sorting = sorting;
            this.Grouping = group;
        }

        public string QueryString { get; }

        public ISort Sorting { get; set; }

        public IGroup Grouping { get; set; }
    }

    public class SearchTrackRequestHandler : IRequestHandler<SearchTrackRequest, ReadOnlyCollection<IGrouping<string, Track>>>
    {
        private readonly SQLiteAsyncConnection connection;

        public SearchTrackRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ReadOnlyCollection<IGrouping<string, Track>>> Handle(SearchTrackRequest request, CancellationToken cancellationToken)
        {
            List<Track>? results;

            if (int.TryParse(request.QueryString, out int year))
            {
                var sql = "SELECT Id, Title, Artist, RecordedYear " +
                    "FROM Track " +
                    "WHERE RecordedYear = ?";

                results = await connection.QueryAsync<Track>(sql, year).ConfigureAwait(false);
            }
            else
            {
                var sql = "SELECT Id, Title, Artist, RecordedYear " +
                    "FROM Track " +
                    "WHERE (Title LIKE ?) OR (Artist LIKE ?)";

                results = await connection.QueryAsync<Track>(sql, $"%{request.QueryString}%", $"%{request.QueryString}%").ConfigureAwait(false);
            }

            var sorted = request.Sorting.Sort(results);
            var groupedAndSorted = request.Grouping.Group(sorted).ToList();

            return groupedAndSorted.AsReadOnly();
        }
    }
}
