using Chroomsoft.Top2000.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Unittests
{
    [TestClass]
    public class Top2000DataTests
    {
        private readonly Top2000Data sut = new Top2000Data();

        [TestMethod]
        public void DataAssemblyIsTheAssemblyOfTheTop2000Data()
        {
            var expected = typeof(Top2000Data).Assembly;
            var actual = sut.DataAssembly;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task AllSqlFilesCanBeRead()
        {
            var filesAsTask = sut.GetAllSqlFiles()
                .Select(GetNameContent);

            var files = await Task.WhenAll(filesAsTask);

            foreach (var file in files)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(file.content),
                    $"The file '{file.name}' does not have content.");
            }
        }

        [TestMethod]
        public void AllSqlFileCanBeStreamed()
        {
            var filesAsStream = sut.GetAllSqlFiles()
                .Select(sut.GetScriptStream);

            foreach (var item in filesAsStream)
            {
                Assert.IsNotNull(item);
                item.Dispose();
            }
        }

        [TestMethod]
        public void FileNamesDoNotContainSpaces()
        {
            var fileNames = sut.GetAllSqlFiles().ToList();

            foreach (var fileName in fileNames)
            {
                Assert.IsFalse(fileName.Contains(' ', System.StringComparison.OrdinalIgnoreCase),
                    $"The file '{fileName}' contains a space");
            }
        }

        private async Task<(string name, string content)> GetNameContent(string fileName)
        {
            var content = await sut.GetScriptContentAsync(fileName);
            return (fileName, content);
        }
    }
}
