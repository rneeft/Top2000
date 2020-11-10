using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Features.Unittests.Searching
{
    [TestClass]
    public class GroupByArtistTests
    {
        [TestMethod]
        public void GroupByArtistGroupsTracksByArtist()
        {
            var trackA_1 = new Track { Artist = "A" };
            var trackA_2 = new Track { Artist = "A" };
            var trackC_1 = new Track { Artist = "C" };
            var trackD_1 = new Track { Artist = "D" };
            var trackD_2 = new Track { Artist = "D" };

            var tracks = new[] { trackD_2, trackA_1, trackA_2, trackC_1, trackD_1 };
            var actual = new GroupByArtist().Group(tracks);

            actual.Should().NotBeEmpty()
                .And.HaveCount(3);

            actual.First().Key.Should().Be("D");
            actual.First().Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.ContainInOrder(trackD_2, trackD_1);

            actual.Skip(1).First().Key.Should().Be("A");
            actual.Skip(1).First().Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.ContainInOrder(trackA_1, trackA_2);

            actual.Skip(2).First().Key.Should().Be("C");
            actual.Skip(2).First().Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.ContainInOrder(trackC_1);
        }
    }
}
