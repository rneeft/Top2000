#nullable enable

using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.ListingDate
{
    public sealed partial class View : Page, YearOverview.IListing
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();

            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; set; }

        public YearOverview.NavigationData? NavigationData { get; set; }

        public async Task TrackListingChange()
        {
            if (NavigationData != null && ViewModel.SelectedListing != null)
                await NavigationData.OnSelectedListingAync((TrackListing)Listing.SelectedItem);
        }

        public void SetListing(TrackListing? listing)
        {
            ViewModel.SelectedListing = listing;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationData = (YearOverview.NavigationData)e.Parameter;

            await ViewModel.LoadListingForEdition(NavigationData.SelectedEdition);

            if (NavigationData.SelectedTrackListing is null)
            {
                ViewModel.SelectedListing = null;
            }
            else
            {
                var listings = ViewModel.Listings
                    .SelectMany(x => x);

                var listing = ViewModel.Listings
                    .SelectMany(x => x)
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
            if (ViewModel.SelectedListing is null) return;

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
