using MediatR;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.AllEditions
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

    public class EditionDescendingComparer : Comparer<Edition>
    {
        public override int Compare(Edition x, Edition y)
        {
            return y.Year - x.Year;
        }
    }

    public class Edition
    {
        public int Year { get; set; }

        public DateTime LocalStartDateAndTime => DateTime.SpecifyKind(Start, DateTimeKind.Utc).ToLocalTime();

        public DateTime LocalEndDateAndTime => DateTime.SpecifyKind(End, DateTimeKind.Utc).ToLocalTime();

        [Column("StartUtcDateAndTime")]
        public DateTime Start { get; set; }

        [Column("EndUtcDateAndTime")]
        public DateTime End { get; set; }
    }
}
