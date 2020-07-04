using Chroomsoft.Top2000.Features.AllEditions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Features.Unittests.AllEditions
{
    [TestClass]
    public class EditionDescendingComparerTests
    {
        [TestMethod]
        public void Editions_with_same_year_compare_as_zero()
        {
            var edition1 = new Edition { Year = 2018 };
            var edition2 = new Edition { Year = 2018 };

            var sut = new EditionDescendingComparer();

            sut.Compare(edition1, edition2).Should().Be(0);
        }

        [TestMethod]
        public void Edition_with_higher_year_compare_as_minus_zero()
        {
            var edition1 = new Edition { Year = 2018 };
            var edition2 = new Edition { Year = 2019 };

            var sut = new EditionDescendingComparer();

            sut.Compare(edition1, edition2).Should().Be(1);
        }

        [TestMethod]
        public void Edition_with_lower_year_compare_as_plus_zero()
        {
            var edition1 = new Edition { Year = 2001 };
            var edition2 = new Edition { Year = 2000 };

            var sut = new EditionDescendingComparer();

            sut.Compare(edition1, edition2).Should().Be(-1);
        }
    }
}
