using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.Overview.Position
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Editions.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }

        private async void OnSelectYearButtonClick(object sender, System.EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await EditionsFlyout.TranslateTo(this.Width * -1, 0, 0);

            EditionsFlyout.IsVisible = true;
            await EditionsFlyout.TranslateTo(0, 0);
        }

        private async void OnJumpGroupButtonClick(object sender, System.EventArgs e)
        {
            var groups = ViewModel.Listings.Select(x => x.Key)
                                         .ToArray();

            var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

            if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
            {
                JumpIntoList(result);
            }
        }

        private async void NewEditionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (AllEditions.SelectedItem is Edition edition)
            {
                ViewModel.SelectedEdition = edition;
                ViewModel.SelectedEditionYear = edition.Year;

                var loadingTask = ViewModel.LoadAllListingsAsync();

                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                await EditionsFlyout.TranslateTo(this.Width * -1, 0);
                this.EditionsFlyout.IsVisible = false;

                await loadingTask;

                JumpIntoList(ViewModel.Listings.First().Key);

                AllEditions.SelectedItem = null;
            }
        }

        private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
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

        private async void OnCloseButtonClick(object sender, EventArgs e)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;
        }
    }
}