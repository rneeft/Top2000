namespace Chroomsoft.Top2000.Apps.Overview.ByPosition;

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

    private void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
    }

    private void OnJumpGroupButtonClick(object sender, TappedEventArgs e)
    {
    }
}