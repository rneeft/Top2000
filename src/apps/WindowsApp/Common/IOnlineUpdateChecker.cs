using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features.DatabaseInfo;
using MediatR;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public interface IOnlineUpdateChecker
    {
        Task<bool> UpdateAsync();
    }

    public class OnlineUpdateChecker : IOnlineUpdateChecker
    {
        private readonly IMediator mediator;
        private readonly IUpdateClientDatabase updateClientDatabase;
        private readonly OnlineDataSource onlineDataSource;
        private readonly IGlobalUpdate globalUpdate;

        public OnlineUpdateChecker(IMediator mediator, IUpdateClientDatabase updateClientDatabase, OnlineDataSource onlineDataSource, IGlobalUpdate globalUpdate)
        {
            this.mediator = mediator;
            this.updateClientDatabase = updateClientDatabase;
            this.onlineDataSource = onlineDataSource;
            this.globalUpdate = globalUpdate;
        }

        public async Task<bool> UpdateAsync()
        {
            try
            {
                var currentVersion = await mediator.Send(new DatabaseInfoRequest());

                await updateClientDatabase.RunAsync(onlineDataSource);

                var newVersion = await mediator.Send(new DatabaseInfoRequest());

                if (currentVersion != newVersion)
                {
                    await globalUpdate.NewVersionAvailableAsync();
                    return true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // lets not crash the app here
            }

            return false;
        }
    }
}
