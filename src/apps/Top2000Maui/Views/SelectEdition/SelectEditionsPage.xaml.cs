using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.SelectEdition;

public partial class SelectEditionsPage : ContentPage
{
    private readonly EditionOnView editionOnView;

    public SelectEditionsPage(SelectEditionsViewModel viewModel, EditionOnView editionOnView)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
        this.editionOnView = editionOnView;
    }

    protected override async void OnAppearing()
    {
        await ((SelectEditionsViewModel)BindingContext).InitialiseViewModelAsync();
    }

    private async void NewEditionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 1)
        {
            editionOnView.SelectedEdition = (Edition)e.CurrentSelection[0];
        }

        await Shell.Current.GoToAsync("..");
    }
}