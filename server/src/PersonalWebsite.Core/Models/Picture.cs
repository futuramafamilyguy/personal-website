using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Core.Models;

public class Picture
{
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required int YearWatched { get; set; }
    public required Month MonthWatched { get; set; }
    public required Cinema Cinema { get; set; }
    public required int YearReleased { get; set; }
    public string? Zinger { get; set; }
    public required bool IsFavorite { get; set; }
    public required bool IsKino { get; set; }
    public required bool IsNewRelease { get; set; }
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public string? AltImageUrl { get; set; }
}
