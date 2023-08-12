using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chroomsoft.Top2000.Data.Unittests;

[TestClass]
public class Top2000DataTests
{
    private readonly Top2000Data sut = new();

    [TestMethod]
    public void DataAssemblyIsTheAssemblyOfTheTop2000Data()
    {
        typeof(Top2000Data).Assembly
            .Should().BeSameAs(sut.DataAssembly);
    }

    [TestMethod]
    public async Task AllSqlFilesCanBeRead()
    {
        var filesAsTask = sut.GetAllSqlFiles()
            .Select(GetNameContent);

        var files = await Task.WhenAll(filesAsTask);

        foreach (var (name, content) in files)
        {
            content.Should().NotBeNullOrWhiteSpace($"The file '{name}' does not have content");
        }
    }

    [TestMethod]
    public void AllSqlFileCanBeStreamed()
    {
        var filesAsStream = sut
            .GetAllSqlFiles()
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
        const string Whitespace = " ";

        var fileNames = sut
            .GetAllSqlFiles()
            .ToList();

        foreach (var fileName in fileNames)
        {
            fileName.Should().NotContain(Whitespace);
        }
    }

    private async Task<(string name, string content)> GetNameContent(string fileName)
    {
        var content = await sut.GetScriptContentAsync(fileName);
        return (fileName, content);
    }
}