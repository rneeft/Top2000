using Chroomsoft.Top2000.Apps.Views.SelectEdition;
using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage
{
    private readonly IUpdateClientDatabase updateClientDatabase;
    private readonly Top2000AssemblyDataSource top2000AssemblyData;
    private readonly EditionOnView editionOnView;
    private bool isInitialise;

    public OverviewByPositionPage(OverviewByPositionViewModel viewModel, IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyData, EditionOnView editionOnView)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
        this.updateClientDatabase = updateClientDatabase;
        this.top2000AssemblyData = top2000AssemblyData;
        this.editionOnView = editionOnView;
    }

    protected override async void OnAppearing()
    {
        var viewModel = (OverviewByPositionViewModel)BindingContext;

        if (!isInitialise)
        {
            await updateClientDatabase.RunAsync(top2000AssemblyData);
            await viewModel.InitialiseViewModelAsync();
            isInitialise = true;
        }
        else
        {
            if (editionOnView.SelectedEdition is not null && editionOnView.SelectedEdition?.Year != viewModel.SelectedEditionYear)
            {
                await viewModel.InitialiseViewModelAsync(editionOnView.SelectedEdition!);
            }
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

    private async void OnSelectYearButtonClick(object sender, TappedEventArgs e)
    {
        var dict = new Dictionary<string, object>();
        await Shell.Current.GoToAsync(nameof(SelectEditionsPage), animate: true, dict);
    }
}