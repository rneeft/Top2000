namespace Chroomsoft.Top2000.Data.ClientDatabase.Models;

public sealed class Edition
{
    [PrimaryKey]
    public int Year { get; set; }

    [Ignore]
    public DateTime LocalStartDateAndTime => DateTime.SpecifyKind(StartUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

    [Ignore]
    public DateTime LocalEndDateAndTime => DateTime.SpecifyKind(EndUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

    public DateTime StartUtcDateAndTime { get; set; }

    public DateTime EndUtcDateAndTime { get; set; }

    public bool HasPlayDateAndTime { get; set; }
}