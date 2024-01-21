namespace Chroomsoft.Top2000.Apps.Views.Overview.ByDate.SelectDateTimeGroup;

public sealed partial class SelectDateTimeGroupViewModel : ObservableObject, IQueryAttributable
{
    public SelectDateTimeGroupViewModel()
    {
        Dates = new();
    }

    public ObservableGroupedCollection<DateTime, DateTime> Dates { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var dates = (ObservableGroupedCollection<DateTime, DateTime>)query["Dates"];
        Dates.ClearAddRange(dates);
    }
}