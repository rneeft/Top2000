namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests.Models;

[TestClass]
public class EditionTests
{
    private readonly Edition sut;

    public EditionTests()
    {
        sut = new Edition();
    }

    [TestMethod]
    public void LocalStartDateAndTimeTransformsTheUtc()
    {
        sut.StartUtcDateAndTime = DateTime.UtcNow;
        sut.LocalStartDateAndTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMicroseconds(100));
    }

    [TestMethod]
    public void LocalEndDateAndTimeTransformsFromUtc()
    {
        sut.EndUtcDateAndTime = DateTime.UtcNow;
        sut.LocalEndDateAndTime.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMicroseconds(100));
    }
}