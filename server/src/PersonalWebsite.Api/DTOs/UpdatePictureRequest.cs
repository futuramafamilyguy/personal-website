namespace PersonalWebsite.Api.DTOs;

public class UpdatePictureRequest
{
    public required string Name { get; set; }
    public required int Year { get; set; }
    public string? Zinger { get; set; }
}
