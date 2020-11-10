using Chroomsoft.Top2000.Features.Searching;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.Searching
{
    [TestClass]
    public class SortByRecordedYearTests
    {
        [TestMethod]
        public void SortByRecordedYearSortsTrackByRecordedYear()
        {
            var trackA = new Track { RecordedYear = 2016 };
            var trackB = new Track { RecordedYear = 2017 };
            var trackC = new Track { RecordedYear = 2018 };
            var trackD = new Track { RecordedYear = 2019 };

            var tracks = new[] { trackB, trackD, trackC, trackA };

            var actual = new SortByRecordedYear().Sort(tracks);
            var expected = new[] { trackA, trackB, trackC, trackD };

            actual.Should().NotBeEmpty()
                .And.HaveCount(4)
                .And.ContainInOrder(expected);
        }
    }
}
