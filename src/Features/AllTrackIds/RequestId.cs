using MediatR;
using SQLite;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.AllTrackIds
{
    public class AllTrackIdRequest : IRequest<ImmutableSortedSet<int>>
    {
    }

    public class AllTrackIdRequestHandler : IRequestHandler<AllTrackIdRequest, ImmutableSortedSet<int>>
    {
        private readonly SQLiteAsyncConnection connection;

        public AllTrackIdRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ImmutableSortedSet<int>> Handle(AllTrackIdRequest request, CancellationToken cancellationToken)
        {
            return (await connection.QueryAsync<Track>("SELECT Id FROM Track ORDER BY Id").ConfigureAwait(false))
                .Select(x => x.Id)
                .ToImmutableSortedSet();
        }

        public class Track
        {
            public int Id { get; set; }
        }
    }

    public class IntDescendingComparer : Comparer<int>
    {
        public override int Compare(int x, int y)
        {
            return y - x;
        }
    }
}
