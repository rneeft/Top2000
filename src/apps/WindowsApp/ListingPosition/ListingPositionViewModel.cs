using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.ListingPosition
{
    public class ListingPositionViewModel : ObservableBase
    {
        public readonly Globals globals;
        private readonly IMediator mediator;

        public ListingPositionViewModel(IMediator mediator, Globals globals)
        {
            this.mediator = mediator;
            this.globals = globals;
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; } = new ObservableGroupedList<string, TrackListing>();

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
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

            //  TrySelectingGlobalListing();
        }

        //public void TrySelectingGlobalListing()
        //{
        //    if (globals.SelectedListing != null)
        //    {
        //        SelectedListing = Listings
        //            .SelectMany(x => x)
        //            .SingleOrDefault(x => x.TrackId == globals.SelectedListing.TrackId);
        //    }
        //}
    }
}
