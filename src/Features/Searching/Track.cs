namespace Chroomsoft.Top2000.Features.Searching;

public sealed class Track
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Artist { get; set; } = string.Empty;

    public int RecordedYear { get; set; }

    public int? Position { get; set; }

    public string LastPosition => $"{Position?.ToString() ?? "-"}";
}