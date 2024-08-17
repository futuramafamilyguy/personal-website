namespace PersonalWebsite.Core.Models;

public class Post
{
    public string? Id { get; set; }
    public required string Title { get; set; }
    public required string ContentUrl { get; set; }
    public string? ImageUrl { get; set; }
    public required DateTime LastUpdatedUtc { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}
