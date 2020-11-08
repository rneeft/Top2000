using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<DateTime, TrackListing>();
            this.Dates = new ObservableGroupedList<DateTime, DateTime>();
        }

        public ObservableGroupedList<DateTime, TrackListing> Listings { get; }

        public ObservableGroupedList<DateTime, DateTime> Dates { get; }

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
            set { SetPropertyValue(value); }
        }

        public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.LocalPlayDateAndTime;

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

        private DateTime LocalPlayDate(DateTime arg) => arg.Date;
    }
}
