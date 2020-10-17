using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }

        public BlankPageViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new BlankPageViewModel();

            if (e.Parameter is TrackListing track)
            {
                await ViewModel.LoadDetails(track);
            }
        }
    }

    public class BlankPageViewModel : ObservableBase
    {
        public string Name
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadDetails(TrackListing track)
        {
            await Task.Delay(1);
            Name = track.Title;
        }
    }
}
