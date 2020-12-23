#nullable enable

using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.ListingPosition
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

        public void OpenCurrentDateAndTime()
        {
            // no implementation inside this class
        }

        public void SetListing(TrackListing? listing)
        {
            ViewModel.SelectedListing = listing;
        }

        public async Task TrackListingChange()
        {
            if (NavigationData != null && ViewModel.SelectedListing != null)
                await NavigationData.OnSelectedListingAync(ViewModel.SelectedListing);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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
            if (Listing.SelectedItem is null) return;
            Listing.ScrollIntoView(Listing.SelectedItem, ScrollIntoViewAlignment.Default);
        }
    }
}
