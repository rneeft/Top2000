namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class ListingInformation
{
    public int Edition { get; set; }

    public int? Position { get; set; }

    public DateTime? PlayUtcDateAndTime { get; set; }

    public DateTime? LocalUtcDateAndTime => PlayUtcDateAndTime is null 
        ? null
        : DateTime.SpecifyKind((DateTime)PlayUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

    public int? Offset { get; set; }

    public ListingStatus Status { get; set; }

    public bool CouldBeListed(int recoredYear) => recoredYear <= Edition;
}