namespace PersonalWebsite.Api.DTOs;

public class UpdatePictureRequest
{
    public required string Name { get; set; }
    public required int Year { get; set; }
    public required string CinemaId { get; set; }
    public string? Zinger { get; set; }
    public string? Alias { get; set; }
    public string? ImageUrl { get; set; }
}
