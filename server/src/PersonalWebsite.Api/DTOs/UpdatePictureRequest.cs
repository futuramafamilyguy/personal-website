using System.ComponentModel.DataAnnotations;
using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Api.DTOs;

public class UpdatePictureRequest
{
    [MinLength(1)]
    public required string Name { get; set; }
    public required int YearWatched { get; set; }
    public Month? MonthWatched { get; set; }
    public required string CinemaId { get; set; }
    public int? YearReleased { get; set; }
    public string? Zinger { get; set; }
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageObjectKey { get; set; }
    public string? AltImageUrl { get; set; }
    public string? AltImageObjectKey { get; set; }
    public bool? IsFavorite { get; set; }
    public bool? IsKino { get; set; }
    public bool? IsNewRelease { get; set; }
}
