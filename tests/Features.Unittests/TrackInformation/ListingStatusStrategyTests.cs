using Chroomsoft.Top2000.Features.TrackInformation;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Features.Unittests.TrackInformation
{
    [TestClass]
    public class ListingStatusStrategyTests
    {
        private ListingStatusStrategy sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = new ListingStatusStrategy(2002);
        }

        [TestMethod]
        public void Status_is_NotAvailable_when_the_track_is_not_recorded()
        {
            var listing = new Listing { Edition = 2000 };

            sut.Determine(listing).Should().Be(ListingStatus.NotAvailable);
        }

        [TestMethod]
        public void Status_is_NotListed_when_track_has_recorded_but_position_is_null()
        {
            var listing = new Listing { Edition = 2002 };

            sut.Determine(listing).Should().Be(ListingStatus.NotListed);
        }

        [TestMethod]
        public void Status_is_New_when_track_has_position_is_filled_and_not_been_set()
        {
            var listing = new Listing { Edition = 2002, Position = 12 };

            sut.Determine(listing).Should().Be(ListingStatus.New);
        }

        [TestMethod]
        public void Status_is_Back_when_previous_status_is_NotListed_and_new_has_been_set()
        {
            new List<Listing>
            {
                new Listing { Edition = 1999, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2000, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2001, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2002, Position = 1, Status = ListingStatus.New},
                new Listing { Edition = 2003, Status = ListingStatus.NotListed},
            }
            .ForEach(x => sut.Determine(x));

            var listing = new Listing { Edition = 2004, Position = 12 };

            sut.Determine(listing).Should().Be(ListingStatus.Back);
        }

        [TestMethod]
        public void Status_is_Unchanged_when_offset_0()
        {
            new List<Listing>
            {
                new Listing { Edition = 1999, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2000, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2001, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2002, Position = 2, Status = ListingStatus.New},
            }
            .ForEach(x => sut.Determine(x));

            var current = new Listing { Edition = 2003, Position = 2, Offset = 0 };
            sut.Determine(current).Should().Be(ListingStatus.Unchanged);
        }

        [TestMethod]
        public void Status_is_Increased_when_offset_negative()
        {
            new List<Listing>
            {
                new Listing { Edition = 1999, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2000, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2001, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2002, Position = 2, Status = ListingStatus.New},
            }
          .ForEach(x => sut.Determine(x));

            var current = new Listing { Edition = 2003, Position = 1, Offset = -1 };
            sut.Determine(current).Should().Be(ListingStatus.Increased);
        }

        [TestMethod]
        public void Status_is_Decreased_when_offset_positive()
        {
            new List<Listing>
            {
                new Listing { Edition = 1999, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2000, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2001, Status = ListingStatus.NotAvailable},
                new Listing { Edition = 2002, Position = 2, Status = ListingStatus.New},
            }
          .ForEach(x => sut.Determine(x));

            var current = new Listing { Edition = 2003, Position = 3, Offset = 1 };
            sut.Determine(current).Should().Be(ListingStatus.Decreased);
        }
    }
}
