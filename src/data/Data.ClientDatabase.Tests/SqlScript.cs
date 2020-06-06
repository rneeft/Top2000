using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests
{
    [TestClass]
    public class SqlScriptTest
    {
        [TestMethod]
        public void ScriptSectionReturnsContentSplitOnSemicolon()
        {
            var contents = "Statement1;Statement2";

            var sut = new SqlScript("TheName", contents);

            var actual = sut.SqlSections();
            var expected = new[] { "Statement1", "Statement2" };

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void EmptySectionAreRemoved()
        {
            var contents = "Statement1;    ;Statement2;";

            var sut = new SqlScript("TheName", contents);

            var actual = sut.SqlSections();
            var expected = new[] { "Statement1", "Statement2" };

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void SqlScriptCanBeInsertedIntoImmutableOrderedSets()
        {
            var scripts = new List<SqlScript>
            {
                new SqlScript("002-File2", string.Empty),
                new SqlScript("010-File10", string.Empty),
                new SqlScript("001-File1", string.Empty),
                new SqlScript("002-File2", string.Empty)
            }
            .ToImmutableSortedSet();

            using (new AssertionScope())
            {
                scripts.Should().HaveCount(3);
                scripts[0].ScriptName.Should().Be("001-File1");
                scripts[1].ScriptName.Should().Be("002-File2");
                scripts[2].ScriptName.Should().Be("010-File10");
            }
        }
    }
}
