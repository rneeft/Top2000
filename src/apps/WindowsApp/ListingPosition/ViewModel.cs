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

        public static string Position(TrackListing listing)
        {
            const int GroupSize = 100;

            if (listing.Position < 100) return "1 - 100";
            if (listing.Position >= 1900) return "1900 - 2000";

            var min = listing.Position / GroupSize * GroupSize;
            var max = min + GroupSize;

            return $"{min} - {max}";
        }

        public async Task LoadListingForEdition(Edition edition)
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(edition.Year));
            var x = tracks.GroupBy(Position);
            Listings.AddRange(x);

            SelectedListing = null;
        }
    }
}
