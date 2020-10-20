using System;

namespace Chroomsoft.Top2000.Features.TrackInformation
{
    public class ListingInformation
    {
        public int Edition { get; set; }

        public int? Position { get; set; }

        public DateTime? PlayUtcDateAndTime { get; set; }

        public int? Offset { get; set; }

        public ListingStatus Status { get; set; }

        public bool CouldBeListed(int recoredYear) => recoredYear <= Edition;
    }
}
