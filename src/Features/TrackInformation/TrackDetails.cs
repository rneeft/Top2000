using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Chroomsoft.Top2000.Features.TrackInformation
{
    public enum ListingStatus
    {
        Unknown = 0,
        New = 1,
        Decreased = 2,
        Increased = 3,
        NotAvailable = 4,
        NotListed = 5,
        Unchanged = 6,
        Back = 7,
    };

    public class ListingInformation
    {
        public int Edition { get; set; }

        public int? Position { get; set; }

        public DateTime? PlayUtcDateAndTime { get; set; }

        public int? Offset { get; set; }

        public ListingStatus Status { get; set; }

        public bool CouldBeListed(int recoredYear) => recoredYear <= Edition;
    }

    public class ListingInformationDescendingComparer : Comparer<ListingInformation>
    {
        public override int Compare(ListingInformation x, ListingInformation y)
        {
            return y.Edition - x.Edition;
        }
    }

    public class TrackDetails
    {
        public TrackDetails(string title, string artist, int recordedYear, ImmutableSortedSet<ListingInformation> listings)
        {
            this.Title = title;
            this.Artist = artist;
            this.RecordedYear = recordedYear;
            this.Listings = listings;
        }

        public string Title { get; }

        public string Artist { get; }

        public int RecordedYear { get; }

        public ImmutableSortedSet<ListingInformation> Listings { get; }

        public ListingInformation Highest => Listings
            .Where(x => x.Position.HasValue)
            .OrderBy(x => x.Position)
            .ThenBy(x => x.Edition)
            .First();

        public ListingInformation Lowest => Listings
            .Where(x => x.Position.HasValue)
            .OrderBy(x => x.Position)
            .ThenBy(x => x.Edition)
            .Last();

        public ListingInformation First => Listings.Single(x => x.Status == ListingStatus.New);

        public ListingInformation Latest => Listings.First(x => x.Position.HasValue);

        public int Appearances => Listings.Count(x => x.Position.HasValue);

        public int AppearancesPossible => Listings.Count(x => x.Status != ListingStatus.NotAvailable);
    }
}
