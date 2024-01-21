using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.AllListingsOfEdition
{
    [TestClass]
    public class TrackListingTests
    {
        [TestMethod]
        public void LocalStartDateAndTimeTransformsTheUtc()
        {
            var sut = new TrackListing
            {
                PlayUtcDateAndTime = DateTime.UtcNow
            };

            sut.LocalPlayDateAndTime.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        }
    }
}