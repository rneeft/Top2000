using Chroomsoft.Top2000.Apps.Views.Overview.ByPosition.SelectEdition;
using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;
using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage, IQueryAttributable
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

    private OverviewByPositionViewModel ViewModel => (OverviewByPositionViewModel)BindingContext;

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("SelectedEdition"))
        {
            var edition = (Edition)query["SelectedEdition"];
            if (edition.Year != ViewModel.SelectedEditionYear)
            {
                await ViewModel.InitialiseViewModelAsync(edition);
            }
        }
    }

    protected override async void OnAppearing()
    {
        if (!isInitialise)
        {
            await updateClientDatabase.RunAsync(top2000AssemblyData);
            await ViewModel.InitialiseViewModelAsync();
            isInitialise = true;
        }
    }

    private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Any())
        {
            var track = (TrackListing)e.CurrentSelection[0];
            await TrackInformationViewModel.NavigateAsync(track.TrackId, track.Title, track.Artist);
            listings.SelectedItem = null;
        }
    }

    private async void OnJumpGroupButtonClick(object sender, EventArgs e)
    {
        var groups = ViewModel.Listings.Select(x => x.Key).ToArray();

        var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

        if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
        {
            JumpIntoList(result);
        }
    }

    private void JumpIntoList(string groupElected)
    {
        var groupIndex = ViewModel.Listings.FindIndex(x => x.Key == groupElected);
        var group = ViewModel.Listings.First(x => x.Key == groupElected);

        var position = group.First().Position;

        const int ShowGroup = 1;
        var index = Math.Max(position + groupIndex - ShowGroup, 0);

        listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
    }

    private async void OnSelectYearButtonClick(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SelectEditionsPage), animate: true);
    }
}