using Chroomsoft.Top2000.Apps.XamarinForms;

namespace Chroomsoft.Top2000.Apps.Searching
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage, ISortGroupNameProvider
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnActivate(this);

            if (string.IsNullOrWhiteSpace(ViewModel.QueryText))
            {
                ViewModel.IsFlat = ViewModel.GroupBy.Value is GroupByNothing;
            }

            GroupSortLayout.IsVisible = false;
            SetTitlesForButton();
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.trackInformation.IsVisible)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }

        private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedTrack is null) return;

            var infoTask = trackInformation.LoadTrackDetailsAsync(ViewModel.SelectedTrack.Id, CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private void ShowSortGroupLayout(object sender, EventArgs e)
        {
            GroupSortLayout.IsVisible = !GroupSortLayout.IsVisible;
        }
    }
}