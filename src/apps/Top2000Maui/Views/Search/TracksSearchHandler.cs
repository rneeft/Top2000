namespace Chroomsoft.Top2000.Apps.Views.Search;

public class TracksSearchHandler : SearchHandler
{
    private SearchViewModel? ViewModel => (SearchViewModel)BindingContext;

    protected override async void OnQueryChanged(string oldValue, string newValue)
    {
        if (ViewModel is not null && newValue is not null && newValue != oldValue && newValue.Length > 2)
        {
            ViewModel.QueryText = newValue;
            await ViewModel.ExceuteSearchAsync();
        }
    }
}