using MediatR;
using SQLite;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features
{
    public class AllEditionsRequest : IRequest<ImmutableSortedSet<Edition>>
    {
    }

    public class AllEditionsRequestHandler : IRequestHandler<AllEditionsRequest, ImmutableSortedSet<Edition>>
    {
        private readonly SQLiteAsyncConnection connection;

        public AllEditionsRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<ImmutableSortedSet<Edition>> Handle(AllEditionsRequest request, CancellationToken cancellationToken)
        {
            return (await connection.Table<Edition>().ToListAsync().ConfigureAwait(false))
                .ToImmutableSortedSet(new EditionDescendingComparer());
        }
    }
}
