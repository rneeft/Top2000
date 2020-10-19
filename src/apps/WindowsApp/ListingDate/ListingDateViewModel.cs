using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public class ListingDateViewModel : ObservableBase
    {
        public readonly Globals globals;
        private readonly IMediator mediator;

        public ListingDateViewModel(IMediator mediator, Globals globals)
        {
            this.mediator = mediator;
            this.globals = globals;
        }

        public ObservableGroupedList<DateTime, TrackListing> Listings { get; } = new ObservableGroupedList<DateTime, TrackListing>();

        public ObservableGroupedList<DateTime, DateTime> Dates { get; } = new ObservableGroupedList<DateTime, DateTime>();

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
            set { SetPropertyValue(value); }
        }

        public static DateTime LocalPlayDateAndTime(TrackListing listing)
        {
            return listing.LocalPlayDateAndTime;
        }

        public async Task LoadListingForEdition(Edition edition)
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(edition.Year));
            var listings = tracks
                .OrderByDescending(x => x.Position)
                .GroupBy(LocalPlayDateAndTime);

            var dates = listings
                .Select(x => x.Key)
                .GroupBy(LocalPlayDate);

            Listings.AddRange(listings);
            Dates.AddRange(dates);
        }

        private DateTime LocalPlayDate(DateTime arg)
        {
            return arg.Date;
        }
    }
}
