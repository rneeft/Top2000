namespace Chroomsoft.Top2000.Features.TrackInformation;

public sealed class TrackDetails
{
    public string Title { get; init; }

    public string Artist { get; init; }

    public int RecordedYear { get; init; }

    public ImmutableSortedSet<ListingInformation> Listings { get; init; }

    public ListingInformation Highest => Listings
        .Where(x => x.Position.HasValue)
        .OrderBy(x => x.Position)
        .ThenBy(x => x.Edition)
        .First();

    public ListingInformation Lowest => Listings
        .Where(x => x.Position.HasValue)
        .OrderBy(x => x.Position)
        .ThenBy(x => x.Edition)
        .Last();

    public ListingInformation First => Listings.Single(x => x.Status == ListingStatus.New);

    public ListingInformation Latest => Listings.First(x => x.Position.HasValue);

    public int Appearances => Listings.Count(x => x.Position.HasValue);

    public int AppearancesPossible => Listings.Count(x => x.Status != ListingStatus.NotAvailable);
}