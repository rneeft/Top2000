using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.Overview.Date
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

        public int SelectedEditionYear
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.LocalPlayDateAndTime;

        public async Task InitialiseViewModelAsync()
        {
            var editions = await mediator.Send(new AllEditionsRequest());
            SelectedEditionYear = editions.First().Year;

            await LoadAllListingsAsync();
        }

        public async Task LoadAllListingsAsync()
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(SelectedEditionYear));
            var listings = tracks
                .OrderByDescending(x => x.Position)
                .GroupBy(LocalPlayDateAndTime);

            var dates = listings
                .Select(x => x.Key)
                .GroupBy(LocalPlayDate);

            Listings.ClearAddRange(listings);
            Dates.ClearAddRange(dates);
        }

        private DateTime LocalPlayDate(DateTime arg) => arg.Date;
    }
}
