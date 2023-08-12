using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator.Unittests;

[TestClass]
public class SqlFileTransformerTests
{
    private readonly Mock<ITop2000AssemblyData> dataMock;
    private readonly SqlFileTransformer sut;

    public SqlFileTransformerTests()
    {
        dataMock = new Mock<ITop2000AssemblyData>();
        sut = new SqlFileTransformer(dataMock.Object);
    }

    [TestMethod]
    public void AllVersionContainsTheListOfAllTheNextVersions()
    {
        dataMock.Setup(x => x.GetAllSqlFiles()).Returns(new HashSet<string>
        {
            "001-FirstVersion.sql",
            "002-SecondVersion.sql",
            "003-ThirdVersion.sql",
        });

        var actual = sut.Transform().ToList();

        Assert.AreEqual(3, actual.Count);
        Assert.AreEqual("001-FirstVersion.sql", actual[0].FileName);
        Assert.AreEqual("001", actual[0].Version);
        Assert.AreEqual(2, actual[0].Upgrades.Count);
        Assert.AreEqual("002-SecondVersion.sql", actual[0].Upgrades.First().FileName);

        Assert.AreEqual("002-SecondVersion.sql", actual[1].FileName);
        Assert.AreEqual("002", actual[1].Version);
        Assert.AreEqual(1, actual[1].Upgrades.Count);
        Assert.AreEqual("003-ThirdVersion.sql", actual[1].Upgrades.Last().FileName);

        Assert.AreEqual("003-ThirdVersion.sql", actual[2].FileName);
        Assert.AreEqual("003", actual[2].Version);
        Assert.AreEqual(0, actual[2].Upgrades.Count);
    }
}