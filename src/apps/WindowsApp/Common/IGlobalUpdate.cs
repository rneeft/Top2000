#nullable enable

using System;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.Common
{
    public interface IGlobalUpdate
    {
        Action? NotificationHandler { get; set; }

        Func<Task>? UpdateListsHandlerAsync { get; set; }

        bool IsUpdateAvalable { get; set; }

        Task NewVersionAvailableAsync();
    }

    public class GlobalUpdates : IGlobalUpdate
    {
        public Action? NotificationHandler { get; set; }

        public Func<Task>? UpdateListsHandlerAsync { get; set; }

        public bool IsUpdateAvalable { get; set; }

        public Task NewVersionAvailableAsync()
        {
            IsUpdateAvalable = true;
            NotificationHandler?.Invoke();

            if (UpdateListsHandlerAsync != null)
            {
                return UpdateListsHandlerAsync.Invoke();
            }

            return Task.CompletedTask;
        }
    }
}
