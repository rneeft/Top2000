using System;

namespace Chroomsoft.Top2000.Features.AllListingsOfEdition
{
    public class TrackListing
    {
        public int TrackId { get; set; }

        public int Position { get; set; }

        public int? Delta { get; set; }

        public DateTime LocalPlayDateAndTime => DateTime.SpecifyKind(PlayUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

        public DateTime PlayUtcDateAndTime { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;
    }
}
