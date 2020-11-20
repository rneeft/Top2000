#nullable enable

using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features.DatabaseInfo;
using Chroomsoft.Top2000.WindowsApp.Common;
using MediatR;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.About
{
    public class ViewModel : ObservableBase
    {
        private readonly IMediator mediator;
        private readonly IUpdateClientDatabase updateClientDatabase;
        private readonly OnlineDataSource onlineDataSource;
        private readonly IGlobalUpdate globalUpdate;

        public ViewModel(IMediator mediator, IUpdateClientDatabase updateClientDatabase, OnlineDataSource onlineDataSource, IGlobalUpdate globalUpdate)
        {
            this.mediator = mediator;
            this.updateClientDatabase = updateClientDatabase;
            this.onlineDataSource = onlineDataSource;
            this.globalUpdate = globalUpdate;
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

        public string ApplicationVersion
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        public async Task CheckForUpdatesAsync()
        {
            IsBusy = true;

            try
            {
                await updateClientDatabase.RunAsync(onlineDataSource);
                var newVersion = await mediator.Send(new DatabaseInfoRequest());

                if (DatabaseVersion != newVersion)
                {
                    await globalUpdate.NewVersionAvailableAsync();
                    DatabaseVersion = newVersion;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // lets not crash the app here
            }

            IsBusy = false;
        }

        public async Task LoadViewModelAsync()
        {
            DatabaseVersion = await mediator.Send(new DatabaseInfoRequest());
        }
    }
}
