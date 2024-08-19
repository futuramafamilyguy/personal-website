namespace PersonalWebsite.Api.DTOs;

public class UpdatePostRequest
{
    public required string Title { get; set; }
    public string? ContentUrl { get; set; }
    public string? ImageUrl { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}
