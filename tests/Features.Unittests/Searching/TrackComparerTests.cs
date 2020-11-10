using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.Searching
{
    [TestClass]
    public class TrackComparerTests
    {
        private readonly TrackComparer sut = new TrackComparer();

        [TestMethod]
        public void TracksAreEqualWhenTrackIdIsTheSame()
        {
            var track1 = new Track { Id = 1 };
            var track2 = new Track { Id = 1 };

            sut.Equals(track1, track2).Should().BeTrue();
        }

        [TestMethod]
        public void TracksDiffentIsTrackIdAreNotTheSame()
        {
            var track1 = new Track { Id = 1 };
            var track2 = new Track { Id = 2 };

            sut.Equals(track1, track2).Should().BeFalse();
        }

        [TestMethod]
        public void HashCodeOfTrackEqualsTrackIdHashCode()
        {
            var track = new Track { Id = 1 };

            sut.GetHashCode(track).Should().Be(track.Id.GetHashCode());
        }
    }
}
