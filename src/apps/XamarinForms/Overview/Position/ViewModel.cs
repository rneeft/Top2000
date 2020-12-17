using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.Overview.Position
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<string, TrackListing>();
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<Edition> Editions { get; }

        public Edition? SelectedEdition
        {
            get { return GetPropertyValue<Edition?>(); }
            set { SetPropertyValue(value); }
        }

        public int SelectedEditionYear
        {
            get { return GetPropertyValue<int>(); }
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

        public async Task InitialiseViewModelAsync()
        {
            var editions = await mediator.Send(new AllEditionsRequest());
            SelectedEdition = editions.First();
            SelectedEditionYear = SelectedEdition.Year;
            Editions.ClearAddRange(editions);

            await LoadAllListingsAsync();
        }

        public async Task LoadAllListingsAsync()
        {
            if (SelectedEdition is null) return;

            var listings = await mediator.Send(new AllListingsOfEditionRequest(SelectedEdition.Year));

            Listings.ClearAddRange(listings.GroupBy(Position));

            SelectedListing = null;
        }
    }
}
