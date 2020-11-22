#nullable enable

using Chroomsoft.Top2000.Features.AllEditions;
using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.YearOverview
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;
        private readonly IGlobalUpdate globalUpdate;

        public ViewModel(IMediator mediator, IGlobalUpdate globalUpdate)
        {
            this.mediator = mediator;
            this.globalUpdate = globalUpdate;
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableList<Edition> Editions { get; }

        public Edition? SelectedEdition
        {
            get { return GetPropertyValue<Edition?>(); }
            set { SetPropertyValue(value); }
        }

        public TrackListing? SelectedTrackListing
        {
            get { return GetPropertyValue<TrackListing?>(); }
            set { SetPropertyValue(value); }
        }

        public bool IsTop2000OnAir
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadAllEditionsAsync()
        {
            if (Editions.Count == 0)
            {
                Editions.AddRange(await mediator.Send(new AllEditionsRequest()));
            }

            if (globalUpdate.IsUpdateAvalable)
            {
                Editions.AddRange(await mediator.Send(new AllEditionsRequest()));
                SelectedEdition = Editions.First();
                globalUpdate.IsUpdateAvalable = false;
            }

            IsTop2000OnAir = Top2000IsOnAir();
        }

        private bool Top2000IsOnAir()
        {
            var edition = Editions.First();
            var time = DateTime.UtcNow;

            return time > edition.StartUtcDateAndTime && time < edition.EndUtcDateAndTime;
        }
    }
}
