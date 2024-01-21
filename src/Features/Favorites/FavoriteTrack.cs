namespace Chroomsoft.Top2000.Features.Favorites;

public sealed class FavoriteTrack : BaseTrack
{
    public int RecordedYear { get; set; }

    public string LastEdition { get; set; } = string.Empty;

    public int? Position { get; set; }

    public string LastPosition => $"{LastEdition}: {Position?.ToString() ?? "-"}";
}