using Chroomsoft.Top2000.Features.Groupings;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Specialized;

namespace Chroomsoft.Top2000App.ViewModels;

[INotifyPropertyChanged]
public partial class PositionsOverviewViewModel
{
    private readonly IAllListingsOfEdition allListingsOfEdition;

    public PositionsOverviewViewModel(IAllListingsOfEdition allListingsOfEdition)
    {
        this.allListingsOfEdition = allListingsOfEdition;
    }

    public ObservableGroupedList<string, TrackListing> Listings { get; } = new();

    [RelayCommand]
    public async Task LoadListingsAsync()
    {
        var listings = await allListingsOfEdition.ListingForEditionAsync(new Edition
        {
            Year = 2022
        });

        Listings.ClearAddRange(new GroupByHundreds().Group(listings));
    }
}

public class ObservableGroupedList<TKey, TItem> : ObservableList<IGrouping<TKey, TItem>> { }

public class ObservableList<TItem> : List<TItem>, INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <summary>
    /// Removed all the items in the list, add the list of items and notify the observers.
    /// </summary>
    /// <param name="items">Items to add</param>
    public void ClearAddRange(IEnumerable<TItem> items)
    {
        this.Clear();
        items.ToList().ForEach(Add);

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
