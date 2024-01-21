namespace Chroomsoft.Top2000.Features.Searching;

public sealed class Track : BaseTrack
{
    public int RecordedYear { get; set; }

    public string LastEdition { get; set; } = string.Empty;

    public int? Position { get; set; }

    public string LastPosition => $"{LastEdition}: {Position?.ToString() ?? "-"}";
}