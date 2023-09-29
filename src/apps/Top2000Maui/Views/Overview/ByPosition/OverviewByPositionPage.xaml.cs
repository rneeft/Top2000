using Chroomsoft.Top2000.Apps.Views.Search;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;
using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage
{
    private readonly IUpdateClientDatabase updateClientDatabase;
    private readonly Top2000AssemblyDataSource top2000AssemblyData;
    private readonly IMediator mediator;
    private bool isInitialise;

    public OverviewByPositionPage(OverviewByPositionViewModel viewModel, IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyData, IMediator mediator)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
        this.updateClientDatabase = updateClientDatabase;
        this.top2000AssemblyData = top2000AssemblyData;
        this.mediator = mediator;
    }

    private OverviewByPositionViewModel ViewModel => (OverviewByPositionViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        if (!isInitialise)
        {
            await updateClientDatabase.RunAsync(top2000AssemblyData);
            await ViewModel.InitialiseViewModelAsync();
            isInitialise = true;
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

    private async void MenuButtonClicked(object sender, EventArgs e)
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        var options = editions.Select(x => x.Year.ToString()).ToArray();
        var result = await this.DisplayActionSheetAsync(AppResources.SelectEdition, AppResources.Cancel, options);

        if (result.IsValid && int.TryParse(result.SelectedOption, out var newYear))
        {
            if (newYear != ViewModel.SelectedEditionYear)
            {
                await ViewModel.InitialiseViewModelAsync(newYear);
            }
        }
    }

    private async void OnSearchButtonClick(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SearchPage), animate: true);
    }

    private void OnFavoriteSwiped(object sender, EventArgs e)
    {
    }
}