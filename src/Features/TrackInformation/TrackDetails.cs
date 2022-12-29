namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class TrackDetails
{
    public required string Title { get; init; }

    public required string Artist { get; init;  }

    public required int RecordedYear { get; init; }

    public required ImmutableSortedSet<Listing> Listings { get; init; }

    public required Listing Highest { get; init; }

    public required Listing Lowest { get; init; }

    public required Listing First { get; init; }

    public required Listing Latest { get; init; }

    public required int Appearances { get; init; }

    public required int AppearancesPossible { get; init; }
}
