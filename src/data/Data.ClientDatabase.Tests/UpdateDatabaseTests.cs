using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLite;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests
{
    [TestClass]
    public class UpdateDatabaseTests
    {
        private const string DatabaseFileName = "unittest.db";
        private SQLiteAsyncConnection connection;
        private UpdateDatabase sut;
        private Mock<ISource> sourceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists(DatabaseFileName))
                File.Delete(DatabaseFileName);

            connection = new SQLiteAsyncConnection("unittest.db");
            sut = new UpdateDatabase(connection);
            sourceMock = new Mock<ISource>();
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await connection.CloseAsync();

            if (File.Exists(DatabaseFileName))
                File.Delete(DatabaseFileName);
        }

        [TestMethod]
        public async Task ForEveryScriptAJournalIsInserted()
        {
            SetupSourceMock(new SqlScript("000-First.sql", "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));"));

            await sut.RunAsync(sourceMock.Object);

            var actuals = (await connection.Table<Journal>().ToListAsync())
                .Select(x => x.ScriptName);

            var expected = new[] { "000-First.sql" };

            actuals.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task ForFaultyScriptsJournalIsNotWritten()
        {
            SetupSourceMock(
                new SqlScript("000-First.sql", "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));"),
                new SqlScript("001-Second.sql", "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));")
            );

            try
            {
                await sut.RunAsync(sourceMock.Object);

                Assert.Fail("Could should have throw exception");
            }
            catch { }

            var expected = new[] { "000-First.sql" };
            var actuals = (await connection.Table<Journal>().ToListAsync())
                .Select(x => x.ScriptName);

            actuals.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task UpdatedJournalListIsSendToSource()
        {
            SetupSourceMock(new SqlScript("000-First.sql", "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));"));

            await sut.RunAsync(sourceMock.Object);

            sourceMock.Setup(x => x.ExecutableScriptsAsync(It.IsAny<IImmutableSet<string>>())).ReturnsAsync(ImmutableSortedSet<SqlScript>.Empty);

            await sut.RunAsync(sourceMock.Object);
            sourceMock.Verify(x => x.ExecutableScriptsAsync(It.Is<IImmutableSet<string>>(req => req.Count == 1 && req.First() == "000-First.sql")), Times.Once);
        }

        private void SetupSourceMock(params SqlScript[] scripts)
        {
            var set = scripts
                .ToImmutableSortedSet();

            sourceMock.Setup(x => x.ExecutableScriptsAsync(It.IsAny<IImmutableSet<string>>())).ReturnsAsync(set);
        }
    }
}
