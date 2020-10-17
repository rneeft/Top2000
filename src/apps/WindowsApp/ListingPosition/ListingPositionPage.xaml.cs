using Chroomsoft.Top2000.Features.AllEditions;
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var edition = (Edition)e.Parameter;

            ViewModel = App.GetService<ListingPositionViewModel>();
            await ViewModel.LoadListingForEdition(edition);
        }
    }
}
