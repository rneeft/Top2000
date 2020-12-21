using Chroomsoft.Top2000.Apps.Globalisation;
using Chroomsoft.Top2000.Apps.XamarinForms;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chroomsoft.Top2000.Apps.Overview.Position
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            FirstStart = true;
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        public bool FirstStart { get; private set; }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Editions.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }

            if (FirstStart)
            {
                FirstStart = false;
                var first = ViewModel.Editions.First().LocalStartDateAndTime;
                var last = ViewModel.Editions.First().LocalEndDateAndTime;
                var current = DateTime.Now;

                if (current > first && current < last)
                {
                    await Shell.Current.GoToAsync("///ViewByDate");
                    return;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.EditionsFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                this.EditionsFlyout.TranslateTo(this.Width * -1, 0);
                this.EditionsFlyout.IsVisible = false;

                return true;
            }

            if (this.trackInformation.IsVisible)
            {
                CloseTrackInformationAsync();
                return true;
            }

            return base.OnBackButtonPressed();
        }

        async private void OnSelectYearButtonClick(object sender, System.EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await EditionsFlyout.TranslateTo(this.Width * -1, 0, 0);

            EditionsFlyout.IsVisible = true;
            await EditionsFlyout.TranslateTo(0, 0);
        }

        async private void OnJumpGroupButtonClick(object sender, System.EventArgs e)
        {
            var groups = ViewModel.Listings.Select(x => x.Key)
                                         .ToArray();

            var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

            if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
            {
                JumpIntoList(result);
            }
        }

        private void JumpIntoList(string groupElected)
        {
            var groupIndex = ViewModel.Listings.FindIndex(x => x.Key == groupElected);
            var group = ViewModel.Listings.Single(x => x.Key == groupElected);

            var position = group.First().Position;

            const int ShowGroup = 1;
            var index = position + groupIndex - ShowGroup;

            if (index < 0) index = 0;

            listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
        }

        async private void NewEditionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedEdition is null) return;

            ViewModel.SelectedEditionYear = ViewModel.SelectedEdition.Year;

            var loadingTask = ViewModel.LoadAllListingsAsync();

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;

            await loadingTask;

            JumpIntoList(ViewModel.Listings.First().Key);
        }

        async private void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedListing is null) return;

            var infoTask = trackInformation.LoadTrackDetailsAsync(ViewModel.SelectedListing.TrackId, CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private async Task CloseTrackInformationAsync()
        {
            ViewModel.SelectedListing = null;

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await this.trackInformation.TranslateTo(this.Width * -1, 0);
            this.trackInformation.IsVisible = false;
        }

        async private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;
        }
    }
}
