namespace Chroomsoft.Top2000.Apps.Views.TrackInformation;

public partial class TrackInformationPage : ContentPage
{
    public TrackInformationPage(TrackInformationViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        await ((TrackInformationViewModel)BindingContext).LoadTrackDetailsAsync();
    }
}