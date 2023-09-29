namespace Chroomsoft.Top2000.Features.AllListingsOfEdition;

public sealed class TrackListing : BaseTrack
{
    public int Position { get; set; }

    public int? Delta { get; set; }

    public DateTime LocalPlayDateAndTime => DateTime.SpecifyKind(PlayUtcDateAndTime, DateTimeKind.Utc).ToLocalTime();

    public DateTime PlayUtcDateAndTime { get; set; }
}