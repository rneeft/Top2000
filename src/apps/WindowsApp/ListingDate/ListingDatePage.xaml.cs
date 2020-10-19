using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.YearOverview;
using System;
using System.Linq;
using Windows.Foundation;
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

        public NavigationData NavigationData { get; set; }

        public void OnSelectionChanged()
        {
            if (Listing.SelectedItem != null)
                NavigationData.OnSelectedListing((TrackListing)Listing.SelectedItem);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel = App.GetService<ListingDateViewModel>();

            NavigationData = (NavigationData)e.Parameter;
            await ViewModel.LoadListingForEdition(NavigationData.SelectedEdition);

            if (NavigationData.SelectedTrackListing != null)
            {
                var listings = ViewModel.Listings
                    .SelectMany(x => x);

                var listing = listings
                    .SingleOrDefault(x => x.TrackId == NavigationData.SelectedTrackListing.TrackId);

                if (listing != null)
                {
                    /* Since the list is ordered by position desc
                     * the 'first' (with Index = 0) item in the list
                     * is position 2000, to come to the correct index
                     * we need to substract 2000 from the selected item's position
                     */

                    var totalCount = listings.Count();
                    var index = totalCount - listing.Position;

                    if (Listing.Items.Count >= index)
                    {
                        Listing.SelectedIndex = index;
                        BringSelectedItemInView();
                    }
                }
            }
        }

        private void BringSelectedItemInView()
        {
            var selectedTime = ViewModel.SelectedListing.LocalPlayDateAndTime;
            var item = ViewModel.Listings.First(x => x.Key.Equals(selectedTime));

            Listing.ScrollIntoView(item, ScrollIntoViewAlignment.Leading);
        }

        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item is TrackListing track)
            {
                Dates.ScrollIntoView(track.LocalPlayDateAndTime, ScrollIntoViewAlignment.Leading);
            }

            if (e.SourceItem.Item is DateTime selectedTime)
            {
                var item = ViewModel.Listings.First(x => x.Key.Equals(selectedTime));
                e.DestinationItem = new SemanticZoomLocation
                {
                    Bounds = new Rect(0, 0, 1, 1),
                    Item = item
                };
            }
        }
    }
}
