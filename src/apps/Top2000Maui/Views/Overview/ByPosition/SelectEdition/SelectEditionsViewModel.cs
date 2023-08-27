using Chroomsoft.Top2000.Features.AllEditions;

namespace Chroomsoft.Top2000.Apps.Views.Overview.ByPosition.SelectEdition;

public sealed partial class SelectEditionsViewModel : ObservableObject
{
    private readonly IMediator mediator;

    public SelectEditionsViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Editions = new();
    }

    public ObservableRangeCollection<Edition> Editions { get; }

    [RelayCommand]
    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        Editions.ClearAddRange(editions);
    }
}