using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Api.DTOs;

public class UpdatePictureRequest
{
    public required string Name { get; set; }
    public required int YearWatched { get; set; }
    public required Month MonthWatched { get; set; }
    public required string CinemaId { get; set; }
    public required int YearReleased { get; set; }
    public string? Zinger { get; set; }
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFavorite { get; set; }
}
