using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition;

public partial class OverviewByPositionPage : ContentPage
{
    private readonly IUpdateClientDatabase updateClientDatabase;
    private readonly Top2000AssemblyDataSource top2000AssemblyData;

    public OverviewByPositionPage(OverviewByPositionViewModel viewModel, IUpdateClientDatabase updateClientDatabase, Top2000AssemblyDataSource top2000AssemblyData)
    {
        BindingContext = viewModel;
        InitializeComponent();
        this.updateClientDatabase = updateClientDatabase;
        this.top2000AssemblyData = top2000AssemblyData;
    }

    protected override async void OnAppearing()
    {
        await updateClientDatabase.RunAsync(top2000AssemblyData);
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