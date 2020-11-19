#nullable enable

using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using System;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public class NavigationData
    {
        public NavigationData(Edition selectedEdition, Func<TrackListing, Task> onSelectedListing)
        {
            SelectedEdition = selectedEdition;
            OnSelectedListingAync = onSelectedListing;
        }

        public Edition SelectedEdition { get; }

        public TrackListing? SelectedTrackListing { get; set; }

        public Func<TrackListing, Task> OnSelectedListingAync { get; }
    }

    public interface IListing
    {
        void SetListing(TrackListing? listing);

        void OpenCurrentDateAndTime();
    }
}
