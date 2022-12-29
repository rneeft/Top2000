namespace Chroomsoft.Top2000App.Pages;

public partial class PositionsOverviewView : ContentPage
{
	public PositionsOverviewView(PositionsOverviewViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
	}

    private async void OnLoaded(object sender, EventArgs e)
    {
        await ((PositionsOverviewViewModel)BindingContext).LoadListingsAsync();
    }
}