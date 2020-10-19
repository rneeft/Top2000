using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public class Globals : ObservableBase
    {
        public Edition SelectedEdition
        {
            get { return GetPropertyValue<Edition>(); }
            set { if (value != null) SetPropertyValue(value); }
        }

        public TrackListing SelectedListing
        {
            get { return GetPropertyValue<TrackListing>(); }
            set { if (value != null) SetPropertyValue(value); }
        }
    }
}
