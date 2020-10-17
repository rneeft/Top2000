using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public sealed partial class ListingDatePage : Page
    {
        public ListingDatePage()
        {
            this.InitializeComponent();
        }

        public ListingDateViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = App.GetService<ListingDateViewModel>();

            var edition = (Edition)e.Parameter;
            await ViewModel.LoadListingForEdition(edition);
        }
    }

    public class ListingDateViewModel : ObservableBase
    {
        private static IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;
        private readonly IMediator mediator;

        public ListingDateViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ObservableGroupedList<string, TrackListing> Listings { get; } = new ObservableGroupedList<string, TrackListing>();

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
            set { SetPropertyValue(value); }
        }

        public static string Date(TrackListing listing)
        {
            var hour = listing.LocalPlayDateAndTime.Hour + 1;
            var date = listing.LocalPlayDateAndTime.ToString("dddd dd MMM H", formatProvider);

            return $"{date}:00 - {hour}:00";
        }

        public async Task LoadListingForEdition(Edition edition)
        {
            var tracks = await mediator.Send(new AllListingsOfEditionRequest(edition.Year));
            var x = tracks.OrderByDescending(y => y.Position).GroupBy(Date);

            Listings.AddRange(x);
        }
    }
}
