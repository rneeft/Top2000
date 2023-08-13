using Chroomsoft.Top2000.Apps.Common;

namespace Chroomsoft.Top2000.Apps.Overview.Position
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Listings = new ObservableGroupedList<string, TrackListing>();
            this.Editions = new ObservableList<Edition>();
        }

        public TrackListing? SelectedListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public ObservableList<Edition> Editions { get; }
    }
}