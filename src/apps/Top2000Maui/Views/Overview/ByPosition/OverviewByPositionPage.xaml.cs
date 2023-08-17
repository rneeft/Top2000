using Chroomsoft.Top2000.Apps.Views.TrackInformation;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage
{
    public OverviewByPositionPage(OverviewByPositionViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        await ((OverviewByPositionViewModel)BindingContext).InitialiseViewModelAsync();
    }

    private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Any())
        {
            var dict = new Dictionary<string, object>
        {
            {  "TrackListing", (TrackListing)e.CurrentSelection[0] }
        };

            await Shell.Current.GoToAsync(nameof(TrackInformationPage), true, dict);
        }
    }

    private void OnJumpGroupButtonClick(object sender, TappedEventArgs e)
    {
    }
}