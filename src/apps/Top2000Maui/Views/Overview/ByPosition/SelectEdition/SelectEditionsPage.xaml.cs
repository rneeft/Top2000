using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition.SelectEdition;

public partial class SelectEditionsPage : ContentPage
{
    public SelectEditionsPage(SelectEditionsViewModel viewModel)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        await ((SelectEditionsViewModel)BindingContext).InitialiseViewModelAsync();
    }

    private async void NewEditionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 1)
        {
            var dict = new Dictionary<string, object>
            {
                {  "SelectedEdition", (Edition)e.CurrentSelection[0] }
            };

            await Shell.Current.GoToAsync("..", animate: true, dict);
        }
        else
        {
            await Shell.Current.GoToAsync("..", animate: true);
        }
    }
}