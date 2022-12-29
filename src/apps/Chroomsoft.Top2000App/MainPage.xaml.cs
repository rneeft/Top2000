namespace Chroomsoft.Top2000App;

public partial class MainPage : ContentPage
{
    public MainPage(PositionsOverviewViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}