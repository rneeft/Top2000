using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Features.Unittests.Searching
{
    [TestClass]
    public class GroupByNothingTests
    {
        [TestMethod]
        public void GroupByNothingPutsAllTrackIntoOneGroupWithTheCountAsKey()
        {
            var trackA = new Track { };
            var trackB = new Track { };
            var trackC = new Track { };
            var trackD = new Track { };

            var tracks = new[] { trackB, trackD, trackC, trackA };
            var actual = new GroupByNothing().Group(tracks);

            actual.Should().NotBeEmpty()
                .And.HaveCount(1);

            actual.First().Key.Should().Be("4");
            actual.First().Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainInOrder(tracks);
        }
    }
}
