using Chroomsoft.Top2000.Features.AllEditions;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.AllEditions
{
    [TestClass]
    public class EditionTests
    {
        private readonly Edition sut = new();

        [TestMethod]
        public void LocalStartDateAndTimeTransformsTheUtc()
        {
            sut.StartUtcDateAndTime = DateTime.UtcNow;
            sut.LocalStartDateAndTime.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        }

        [TestMethod]
        public void LocalEndDateAndTimeTransformsFromUtc()
        {
            sut.EndUtcDateAndTime = DateTime.UtcNow;
            sut.LocalEndDateAndTime.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        }
    }
}