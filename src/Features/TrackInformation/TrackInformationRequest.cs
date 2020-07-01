using MediatR;
using SQLite;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Features
{
    public class TrackInformationRequest : IRequest<TrackInformation>
    {
        private readonly int trackId;

        public TrackInformationRequest(int trackId)
        {
            this.trackId = trackId;
        }
    }

    public class TrackInformationRequestHandler : IRequestHandler<TrackInformationRequest, TrackInformation>
    {
        private readonly SQLiteAsyncConnection connection;

        public TrackInformationRequestHandler(SQLiteAsyncConnection connection)
        {
            this.connection = connection;
        }

        public Task<TrackInformation> Handle(TrackInformationRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TrackInformation
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        public int Year { get; set; }
    }
}
