namespace PersonalWebsite.Api.DTOs;

public class CreatePictureRequest
{
    public required string Name { get; set; }
    public required string CinemaId { get; set; }
    public string? Zinger { get; set; }
}
