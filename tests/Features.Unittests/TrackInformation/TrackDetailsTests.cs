using Chroomsoft.Top2000.Features.TrackInformation;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Features.Unittests.TrackInformation
{
    [TestClass]
    public class TrackDetailsTests
    {
        private ListingInformation listing1999;
        private ListingInformation listing2000;
        private ListingInformation listing2001;
        private ListingInformation listing2002;
        private ListingInformation listing2003;
        private ListingInformation listing2004;
        private ListingInformation listing2005;
        private TrackDetails sut;

        [TestInitialize]
        public void TestInitialize()
        {
            listing1999 = new ListingInformation { Edition = 1999, Status = ListingStatus.NotAvailable };
            listing2000 = new ListingInformation { Edition = 2000, Position = 10, Status = ListingStatus.New };
            listing2001 = new ListingInformation { Edition = 2001, Position = 2, Status = ListingStatus.Increased };
            listing2002 = new ListingInformation { Edition = 2002, Position = 2, Status = ListingStatus.Unchanged };
            listing2003 = new ListingInformation { Edition = 2003, Position = 10, Status = ListingStatus.Decreased };
            listing2004 = new ListingInformation { Edition = 2004, Position = 9, Status = ListingStatus.Increased };
            listing2005 = new ListingInformation { Edition = 2005, Status = ListingStatus.NotListed };

            var set = new List<ListingInformation>
            {
                listing1999,
                listing2000,
                listing2001,
                listing2002,
                listing2003,
                listing2004,
                listing2005
            }.ToImmutableSortedSet(new ListingInformationDescendingComparer());

            sut = new TrackDetails("unit_title", "unit_artist", 2000, set);
        }

        [TestMethod]
        public void First_represents_the_first_time_the_track_was_listed()
        {
            sut.First.Should().Be(listing2000);
        }

        [TestMethod]
        public void Highest_represents_the_first_highest_listing()
        {
            sut.Highest.Should().Be(listing2001);
        }

        [TestMethod]
        public void Lowest_represents_the_latested_lowest_listing()
        {
            sut.Lowest.Should().Be(listing2003);
        }

        [TestMethod]
        public void Latest_represents_the_latest_listing()
        {
            sut.Latest.Should().Be(listing2004);
        }

        [TestMethod]
        public void Appearences_shows_the_counts_of_listings()
        {
            sut.Appearances.Should().Be(5);
        }

        [TestMethod]
        public void AppearancesPossible_shows_the_count_of_listings_possible_since_the_recorded_date()
        {
            sut.AppearancesPossible.Should().Be(6);
        }
    }
}
