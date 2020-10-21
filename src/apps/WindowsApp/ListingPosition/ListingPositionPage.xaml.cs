using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.YearOverview;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.ListingPosition
{
    public sealed partial class ListingPositionPage : Page
    {
        public ListingPositionPage()
        {
            this.InitializeComponent();
        }

        public ListingPositionViewModel ViewModel { get; set; }

        public NavigationData NavigationData { get; set; }

        public async Task TrackListingChange()
        {
            if (Listing.SelectedItem != null)
                await NavigationData.OnSelectedListingAync((TrackListing)Listing.SelectedItem);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationData = (NavigationData)e.Parameter;

            ViewModel = App.GetService<ListingPositionViewModel>();
            await ViewModel.LoadListingForEdition(NavigationData.SelectedEdition);

            if (NavigationData.SelectedTrackListing != null)
            {
                var listing = ViewModel.Listings
                    .SelectMany(x => x)
                    .SingleOrDefault(x => x.TrackId == NavigationData.SelectedTrackListing.TrackId);

                if (listing != null)
                {
                    Listing.SelectedIndex = listing.Position - 1;
                    BringSelectedItemInView();
                }
            }
        }

        private void BringSelectedItemInView()
        {
            Listing.ScrollIntoView(Listing.SelectedItem, ScrollIntoViewAlignment.Default);
        }
    }
}
