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

    private async void OnViewVideoClick(object sender, EventArgs e)
    {
        var viewModel = (TrackInformationViewModel)BindingContext;
        var trackTitle = viewModel.Title.Replace(' ', '+');
        var artistName = viewModel.Artist.Replace(' ', '+');

        var url = new Uri($"https://duckduckgo.com/?q=!ducky+onsite:www.youtube.com+{trackTitle}+{artistName}");

        await Launcher.OpenAsync(url);
    }
}