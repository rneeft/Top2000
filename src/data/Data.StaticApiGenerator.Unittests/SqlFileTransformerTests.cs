﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Chroomsoft.Top2000.Data.StaticApiGenerator.Unittests
{
    [TestClass]
    public class SqlFileTransformerTests
    {
        private Mock<ITop2000AssemblyData> dataMock;
        private SqlFileTransformer sut;

        [TestInitialize]
        public void MyTestMethod()
        {
            dataMock = new Mock<ITop2000AssemblyData>();
            sut = new SqlFileTransformer(dataMock.Object);
        }

        [TestMethod]
        public void AllVersionContainsTheListOfAllTheNextVersions()
        {
            dataMock.Setup(x => x.GetAllSqlFiles()).Returns(new List<string>
            {
                "1-FirstVersion.sql",
                "2-SecondVersion.sql",
                "3-ThirdVersion.sql",
            });

            var actual = sut.Transform().ToList();

            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("1-FirstVersion.sql", actual[0].FileName);
            Assert.AreEqual(1, actual[0].Version);
            Assert.AreEqual(2, actual[0].Upgrades.Count);
            Assert.AreEqual("2-SecondVersion.sql", actual[0].Upgrades.First().FileName);

            Assert.AreEqual("2-SecondVersion.sql", actual[1].FileName);
            Assert.AreEqual(2, actual[1].Version);
            Assert.AreEqual(1, actual[1].Upgrades.Count);
            Assert.AreEqual("3-ThirdVersion.sql", actual[1].Upgrades.Last().FileName);

            Assert.AreEqual("3-ThirdVersion.sql", actual[2].FileName);
            Assert.AreEqual(3, actual[2].Version);
            Assert.AreEqual(0, actual[2].Upgrades.Count);
        }
    }
}
