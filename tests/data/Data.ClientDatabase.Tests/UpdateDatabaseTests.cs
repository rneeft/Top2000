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
            sourceMock
                .Setup(x => x.ExecutableScriptsAsync(It.IsAny<ImmutableSortedSet<string>>()))
                .ReturnsAsync(Create.ImmutableSortedSetFrom("000-First.sql"));

            sourceMock.Setup(x => x.ScriptContentsAsync("000-First.sql")).ReturnsAsync(new SqlScript("000-First.sql", string.Empty));

            await sut.RunAsync(sourceMock.Object);
            sourceMock.Verify(x => x.ScriptContentsAsync("000-First.sql"), Times.Once);

            var actuals = (await connection.Table<Journal>().ToListAsync())
                .Select(x => x.ScriptName);

            var expected = new[] { "000-First.sql" };

            actuals.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task AllSectionOfTheScriptAreExecuted()
        {
            sourceMock
                .Setup(x => x.ExecutableScriptsAsync(It.IsAny<ImmutableSortedSet<string>>()))
                .ReturnsAsync(Create.ImmutableSortedSetFrom("000-First.sql"));

            var sql = "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));INSERT INTO Table1(Id) VALUES (1),(2);";
            sourceMock.Setup(x => x.ScriptContentsAsync("000-First.sql")).ReturnsAsync(new SqlScript("000-First.sql", sql));

            await sut.RunAsync(sourceMock.Object);

            var table1 = await connection.QueryAsync<int>("SELECT * FROM Table1");
            table1.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task ForFaultyScriptsJournalIsNotWritten()
        {
            sourceMock
                .Setup(x => x.ExecutableScriptsAsync(It.IsAny<ImmutableSortedSet<string>>()))
                .ReturnsAsync(Create.ImmutableSortedSetFrom("000-First.sql", "001-Second.sql"));

            var sql1 = "CREATE TABLE Table1(Id INT NOT NULL, PRIMARY KEY(Id));INSERT INTO Table1(Id) VALUES (1),(2);";
            var sql2 = "INSERT INTO Table1(Id) VALUES ('2')";
            sourceMock.Setup(x => x.ScriptContentsAsync("000-First.sql")).ReturnsAsync(new SqlScript("000-First.sql", sql1));
            sourceMock.Setup(x => x.ScriptContentsAsync("001-Second.sql")).ReturnsAsync(new SqlScript("001-Second.sql", sql2));

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
    }
}
