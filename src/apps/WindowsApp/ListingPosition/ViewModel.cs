#nullable enable

using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.ListingPosition
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<string, TrackListing>();
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public int CountOfItems
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public string Position(TrackListing listing)
        {
            const int GroupSize = 100;

            if (listing.Position < 100) return "1 - 100";

            if (CountOfItems > 2000 || CountOfItems == 500)
            {
                if (listing.Position >= 2400) return "2400 - 2500";
            }
            else
            {
                if (listing.Position >= 1900) return "1900 - 2000";
            }

            var min = listing.Position / GroupSize * GroupSize;
            var max = min + GroupSize;

            return $"{min} - {max}";
        }

        public async Task LoadListingForEdition(Edition edition)
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(edition.Year));
            CountOfItems = tracks.Count;
            var x = tracks.GroupBy(Position);
            Listings.ClearAddRange(x);

            SelectedListing = null;
        }
    }
}
