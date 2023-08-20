using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage
{
    private readonly IUpdateClientDatabase updateClientDatabase;
    private readonly Top2000AssemblyDataSource top2000AssemblyData;
    private bool isInitialise;

    public OverviewByPositionPage(OverviewByPositionViewModel viewModel, IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyData)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
        this.updateClientDatabase = updateClientDatabase;
        this.top2000AssemblyData = top2000AssemblyData;
    }

    protected override async void OnAppearing()
    {
        if (!isInitialise)
        {
            await updateClientDatabase.RunAsync(top2000AssemblyData);
            await ((OverviewByPositionViewModel)BindingContext).InitialiseViewModelAsync();
            isInitialise = true;
        }
    }

    private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Any())
        {
            var dict = new Dictionary<string, object>
            {
                {  "TrackListing", (TrackListing)e.CurrentSelection[0] }
            };

            await Shell.Current.GoToAsync(nameof(TrackInformationPage), animate: true, dict);
        }
    }

    private async void OnJumpGroupButtonClick(object sender, EventArgs e)
    {
        var viewModel = (OverviewByPositionViewModel)BindingContext;

        var groups = viewModel.Listings.Select(x => x.Key).ToArray();

        var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

        if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
        {
            JumpIntoList(result);
        }
    }

    private void JumpIntoList(string groupElected)
    {
        var viewModel = (OverviewByPositionViewModel)BindingContext;
        var groupIndex = viewModel.Listings.FindIndex(x => x.Key == groupElected);
        var group = viewModel.Listings.Single(x => x.Key == groupElected);

        var position = group.First().Position;

        const int ShowGroup = 1;
        var index = position + groupIndex - ShowGroup;

        if (index < 0)
        {
            index = 0;
        }

        listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
    }
}