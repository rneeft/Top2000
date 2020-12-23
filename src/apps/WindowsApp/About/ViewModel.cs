#nullable enable

using Chroomsoft.Top2000.Features.DatabaseInfo;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.About
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;
        private readonly IOnlineUpdateChecker onlineUpdateChecker;

        public ViewModel(IMediator mediator, IOnlineUpdateChecker onlineUpdateChecker)
        {
            this.mediator = mediator;
            this.onlineUpdateChecker = onlineUpdateChecker;
        }

        public bool IsBusy
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        public int DatabaseVersion
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        public async Task CheckForUpdatesAsync()
        {
            IsBusy = true;

            var isUpdated = await this.onlineUpdateChecker.UpdateAsync();

            if (isUpdated)
                DatabaseVersion = await mediator.Send(new DatabaseInfoRequest());

            IsBusy = false;
        }

        public async Task LoadViewModelAsync()
        {
            DatabaseVersion = await mediator.Send(new DatabaseInfoRequest());
        }
    }
}
