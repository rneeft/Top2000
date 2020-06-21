using MediatR;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsApp.Features
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
                .ToImmutableSortedSet(new EditionComparer());
        }
    }

    public class EditionComparer : IComparer<Edition>
    {
        public int Compare(Edition x, Edition y)
        {
            return x.Year.CompareTo(y.Year);
        }
    }

    public class Edition
    {
        public int Year { get; set; }

        public DateTimeOffset StartDateAndTime { get; set; }

        public DateTimeOffset EndDateAndTime { get; set; }
    }
}
