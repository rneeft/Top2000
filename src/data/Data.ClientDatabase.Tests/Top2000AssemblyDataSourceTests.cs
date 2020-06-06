using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests
{
    [TestClass]
    public class Top2000AssemblyDataSourceTests
    {
        private Mock<ITop2000AssemblyData> dataMock;
        private Top2000AssemblyDataSource sut;

        [TestInitialize]
        public void TestInitialize()
        {
            dataMock = new Mock<ITop2000AssemblyData>();
            sut = new Top2000AssemblyDataSource(dataMock.Object);
        }

        [TestMethod]
        public async Task ExecutableScriptsAreComingFromTheDataAssembly()
        {
            var set = new HashSet<string>();
            dataMock.Setup(x => x.GetAllSqlFiles()).Returns(set);

            var scripts = await sut.ExecutableScriptsAsync();

            scripts.Equals(set);
        }

        [TestMethod]
        public async Task ScriptContentsIsContentFromTheDataAssemblyTransformedInASqlScriptClass()
        {
            var scriptName = "001-SqlScript.sql";
            var contents = "CREATE TABLE(id INT);";
            dataMock.Setup(x => x.GetScriptContentAsync(scriptName)).ReturnsAsync(contents);

            var sqlScript = await sut.ScriptContentsAsync(scriptName);

            sqlScript.ScriptName.Should().Be(scriptName);
            sqlScript.Contents.Should().Be(contents);
        }
    }
}
