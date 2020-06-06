using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        //[TestMethod]
        //public void SqlScriptCanBeInsertedIntoImmutableOrderedSets()
        //{
        //    var scripts = new List<SqlScript>
        //    {
        //        new SqlScript("002-File2", string.Empty),
        //        new SqlScript("010-File10", string.Empty),
        //        new SqlScript("001-File1", string.Empty),
        //        new SqlScript("002-File2", string.Empty)
        //    }
        //    .ToImmutableSortedSet();

        //    using (new AssertionScope())
        //    {
        //        scripts.Should().HaveCount(3);
        //        scripts[0].ScriptName.Should().Be("001-File1");
        //        scripts[1].ScriptName.Should().Be("002-File2");
        //        scripts[2].ScriptName.Should().Be("010-File10");
        //    }
        //}

        //[TestMethod]
        //public void GetHashCodeIsEqualToHashCodeForSqlScript()
        //{
        //    var scriptName = "010-ScriptName.sql";
        //    var sut = new SqlScript(scriptName, string.Empty);

        //    sut.GetHashCode().Should().Be(scriptName.GetHashCode());
        //}

        //[TestMethod]
        //public void ScriptFileIsEqualWhenNameAndContentAreEqual()
        //{
        //    var script1 = new SqlScript("A", "B");
        //    var script2 = new SqlScript("A", "B");
        //    var script3 = new SqlScript("A", "C");
        //    var script4 = new SqlScript("D", "B");

        //    script1.Equals(script2).Should().BeTrue();
        //    script1.Equals(script3).Should().BeFalse();
        //    script1.Equals(script4).Should().BeFalse();
        //}
    }
}
