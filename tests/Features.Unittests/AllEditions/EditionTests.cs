using Chroomsoft.Top2000.Features;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Features.Unittests.AllEditions
{
    [TestClass]
    public class EditionTests
    {
        private Edition sut;

        [TestInitialize]
        public void TestInitialize()
        {
            sut = new Edition();
        }

        [TestMethod]
        public void LocalStartDateAndTimeTransformsTheUtc()
        {
            sut.StartUtcDateAndTime = DateTime.UtcNow;
            sut.LocalStartDateAndTime.Should().BeCloseTo(DateTime.Now);
        }

        [TestMethod]
        public void LocalEndDateAndTimeTransformsFromUtc()
        {
            sut.EndUtcDateAndTime = DateTime.UtcNow;
            sut.LocalEndDateAndTime.Should().BeCloseTo(DateTime.Now);
        }
    }
}
