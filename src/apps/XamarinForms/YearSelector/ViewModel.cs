using Chroomsoft.Top2000.Apps.Common;
using Chroomsoft.Top2000.Features.AllEditions;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.YearSelector
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;

        public ViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            this.Editions = new ObservableList<Edition>();
        }

        public ObservableList<Edition> Editions { get; }

        public Edition? SelectedEdition
        {
            get { return GetPropertyValue<Edition?>(); }
            set { SetPropertyValue(value); }
        }

        public async Task LoadAllEditionsAsync(int year)
        {
            Editions.ClearAddRange(await mediator.Send(new AllEditionsRequest()));
            TrySelectingEdition(year);
        }

        private void TrySelectingEdition(int year)
        {
            SelectedEdition = Editions.SingleOrDefault(x => x.Year == year);
        }
    }
}
