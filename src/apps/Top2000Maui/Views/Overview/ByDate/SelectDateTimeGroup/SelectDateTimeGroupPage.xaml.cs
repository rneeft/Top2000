namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate.SelectDateTimeGroup;

public partial class SelectDateTimeGroupPage : ContentPage
{
    public SelectDateTimeGroupPage(SelectDateTimeGroupViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 1)
        {
            var dict = new Dictionary<string, object>
            {
                {  "SelectedDate", (DateTime)e.CurrentSelection[0] }
            };

            await Shell.Current.GoToAsync("..", animate: true, dict);
        }
    }
}