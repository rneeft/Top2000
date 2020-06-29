using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.AllListingsOfEdition
{
    [TestClass]
    public class TrackListingComparerTests
    {
        [TestMethod]
        public void TrackListingAreEqualWhenPositionIsTheSame()
        {
            var sut = new TrackListingComparer();
            var track1 = new TrackListing { Position = 1 };
            var track2 = new TrackListing { Position = 1 };

            sut.Equals(track1, track2).Should().BeTrue();
        }

        [TestMethod]
        public void HashCodeOfTrackListingEqualsPositionHashCode()
        {
            var sut = new TrackListingComparer();
            var track = new TrackListing { Position = 1392 };

            sut.GetHashCode(track).Should().Be(track.Position.GetHashCode());
        }
    }
}
