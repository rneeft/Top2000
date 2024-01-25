using MediatR;
using SQLite;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features.DatabaseInfo
{
    public class DatabaseInfoRequest : IRequest<int>
    {
    }

    public class DatabaseInfoRequestHandler : IRequestHandler<DatabaseInfoRequest, int>
    {
        private readonly SQLiteAsyncConnection connection;

        public DatabaseInfoRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Handle(DatabaseInfoRequest request, CancellationToken cancellationToken)
        {
            var sql = "SELECT ScriptName FROM Journal ORDER BY ScriptName DESC LIMIT 1";

            var idString = (await connection.QueryAsync<Journal>(sql).ConfigureAwait(false))
                .Single()
                .ScriptName.Split('-')[0];

            if (int.TryParse(idString, out int id))
            {
                return id;
            }

            return 0;
        }

        private class Journal
        {
            public string ScriptName { get; set; } = string.Empty;
        }
    }
}
