using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public sealed partial class View : Page
    {
        public View()
        {
            ViewModel = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel { get; set; }

        public void GoLeft() => ChangeView(-1);

        public void GoRight() => ChangeView(1);

        public async Task LoadNewTrackAsync(TrackListing listing)
        {
            await ViewModel.LoadTrackDetails(listing);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var selectedTrackListing = (TrackListing)e.Parameter;

            await ViewModel.LoadTrackDetails(selectedTrackListing);
        }

        private void ChangeView(int x)
        {
            var width = PositionScrollViewer.ActualWidth * x;
            var newOffSet = PositionScrollViewer.HorizontalOffset + width;
            PositionScrollViewer.ChangeView(newOffSet, verticalOffset: null, zoomFactor: null);

            CheckButtonState(newOffSet);
        }

        private void CheckButtonState(double newOffSet)
        {
            GoLeftButton.IsEnabled = newOffSet > 0;
            var rightOffset = newOffSet + (PositionScrollViewer.ActualWidth * 1);
            GoRightButton.IsEnabled = rightOffset < TrackPositions.ActualWidth;
        }

        private async Task OpenOnYoutube()
        {
            var trackTitle = ViewModel.Title.Replace(' ', '+');
            var artistName = ViewModel.Artist.Replace(' ', '+');

            var url = new Uri($"https://duckduckgo.com/?q=!ducky+onsite:www.youtube.com+{trackTitle}+{artistName}");

            await Launcher.LaunchUriAsync(url);
        }
    }
}
