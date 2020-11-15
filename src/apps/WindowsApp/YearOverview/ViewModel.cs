#nullable enable

using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public ObservableList<Edition> Editions { get; } = new ObservableList<Edition>();

        public Edition SelectedEdition
        {
            get { return GetPropertyValue<Edition>(); }
            set { SetPropertyValue(value); }
        }

        public TrackListing? SelectedTrackListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadAllEditionsAsync()
        {
            Editions.AddRange(await mediator.Send(new AllEditionsRequest()));
        }
    }
}
