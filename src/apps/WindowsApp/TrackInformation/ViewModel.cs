using Chroomsoft.Top2000.Features.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableList<ListingInformation>();
        }

        public string Title
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public string Artist
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public int RecordedYear
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<ListingInformation> Listings { get; }

        public ListingInformation Highest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation Lowest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation Latest
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public ListingInformation First
        {
            get { return GetPropertyValue<ListingInformation>(); }
            set { SetPropertyValue(value); }
        }

        public int Appearances
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public bool IsLatestListed
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public int AppearancesPossible
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadTrackDetails(int trackId)
        {
            var track = await mediator.Send(new TrackInformationRequest(trackId));

            Title = track.Title;
            Artist = track.Artist;
            RecordedYear = track.RecordedYear;
            Highest = track.Highest;
            Lowest = track.Lowest;
            Latest = track.Latest;
            First = track.First;
            Appearances = track.Appearances;
            AppearancesPossible = track.AppearancesPossible;
            IsLatestListed = track.Listings.First().Status != ListingStatus.NotListed;
            Listings.Clear();
            Listings.ClearAddRange(track.Listings);
        }
    }
}
