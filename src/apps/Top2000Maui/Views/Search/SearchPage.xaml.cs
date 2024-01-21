using Chroomsoft.Top2000.Apps.Views.TrackInformation;
using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Apps.Views.Search;

public partial class SearchPage : ContentPage, ISortGroupNameProvider
{
    public SearchPage(SearchViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    public SearchViewModel ViewModel => (SearchViewModel)BindingContext;

    public string GetNameForGroup(IGroup group)
    {
        return GetNameForGroupOrSortBy(group)
            ?? throw new NotImplementedException($"Group '{group.GetType()}' was not defined yet.");
    }

    public string GetNameForSort(ISort sort)
    {
        return GetNameForGroupOrSortBy(sort)
            ?? throw new NotImplementedException($"Display text for sort option '{sort.GetType()}' was not defined.");
    }

    protected override void OnAppearing()
    {
        ViewModel.OnActivate(this);
        SetTitlesForButton();
    }

    private static string? GetNameForGroupOrSortBy(object item) => item switch
    {
        GroupByNothing _ => AppResources.None,
        SortByArtist _ => AppResources.Artist,
        GroupByArtist _ => AppResources.Artist,
        SortByRecordedYear _ => AppResources.Year,
        GroupByRecordedYear _ => AppResources.Year,
        SortByTitle _ => AppResources.Title,
        _ => "",
    };

    private void SetTitlesForButton()
    {
        GroupByButton.Text = $"{Translator.Instance["GroupByHeader"]} {ViewModel.GroupBy.Name}";
        SortByButton.Text = $"{Translator.Instance["SortByHeader"]} {ViewModel.SortBy.Name}";
    }

    private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Any())
        {
            var track = (Track)e.CurrentSelection[0];
            await TrackInformationViewModel.NavigateAsync(track);
            listingsGrouped.SelectedItem = null;
        }
    }

    private void ShowSortGroupLayout(object sender, EventArgs e)
    {
        GroupSortLayout.IsVisible = !GroupSortLayout.IsVisible;
    }

    private async void OnGroupByButtonClick(object sender, System.EventArgs e)
    {
        var groups = ViewModel.GroupOptions.Select(x => x.Name).ToArray();

        var actionResult = await this.DisplayActionSheetAsync(AppResources.GroupByHeader, AppResources.Cancel, groups);

        if (actionResult.IsValid)
        {
            GroupByButton.Text = $"{Translator.Instance["GroupByHeader"]} {actionResult.SelectedOption}";

            var groupBy = ViewModel.GroupOptions.SingleOrDefault(x => x.Name == actionResult.SelectedOption);
            if (groupBy != null)
            {
                ViewModel.GroupBy = groupBy;
                ViewModel.ReSortGroup();
            }
        }
    }

    private async void OnSortByButtonClick(object sender, System.EventArgs e)
    {
        var sortings = ViewModel.SortOptions.Select(x => x.Name).ToArray();

        var toSort = await DisplayActionSheet(AppResources.SortByHeader, AppResources.Cancel, null, sortings);

        if (!string.IsNullOrEmpty(toSort) && toSort != AppResources.Cancel)
        {
            SortByButton.Text = $"{Translator.Instance["SortByHeader"]} {toSort}";

            var sortBy = ViewModel.SortOptions.SingleOrDefault(x => x.Name == toSort);
            if (sortBy != null)
            {
                ViewModel.SortBy = sortBy;
                ViewModel.ReSortGroup();
            }
        }
    }
}