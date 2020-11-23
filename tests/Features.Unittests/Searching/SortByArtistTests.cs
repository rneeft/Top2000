using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.Searching
{
    [TestClass]
    public class SortByArtistTests
    {
        [TestMethod]
        public void SortByArtistSortTrackByTheNameOfTheArtistAlphabetically()
        {
            var trackA = new Track { Artist = "A" };
            var trackB = new Track { Artist = "B" };
            var trackC = new Track { Artist = "C" };
            var trackD = new Track { Artist = "D" };

            var tracks = new[] { trackB, trackD, trackC, trackA };

            var actual = new SortByArtist().Sort(tracks);
            var expected = new[] { trackA, trackB, trackC, trackD };

            actual.Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainInOrder(expected);
        }
    }
}
