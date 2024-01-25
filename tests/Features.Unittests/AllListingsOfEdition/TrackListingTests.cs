using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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

            sut.LocalPlayDateAndTime.Should().BeCloseTo(DateTime.Now);
        }
    }
}
