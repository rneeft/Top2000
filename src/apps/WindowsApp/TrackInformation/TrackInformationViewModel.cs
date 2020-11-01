using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.Features.TrackInformation;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using MoreLinq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public class TrackInformationViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public TrackInformationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
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

        public ObservableCollection<ListingInformation> Listings { get; } = new ObservableCollection<ListingInformation>();

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

        public async Task LoadTrackDetails(TrackListing trackListing)
        {
            var track = await mediator.Send(new TrackInformationRequest(trackListing.TrackId));
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
            foreach (var item in track.Listings)
            {
                Listings.Add(item);
            }
        }
    }
}
