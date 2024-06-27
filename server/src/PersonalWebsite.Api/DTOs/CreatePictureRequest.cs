using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Api.DTOs;

public class CreatePictureRequest
{
    public required string Name { get; set; }
    public required string CinemaId { get; set; }
    public Month? MonthWatched { get; set; }
    public string? Zinger { get; set; }
    public string? Alias { get; set; }
}
