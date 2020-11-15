using System;

namespace Chroomsoft.Top2000.WindowsApp.TrackInformation
{
    public class NavigationData
    {
        public NavigationData(int trackId, Action onClose)
        {
            TrackId = trackId;
            OnClose = onClose;
        }

        public int TrackId { get; }

        public Action OnClose { get; }
    }
}
