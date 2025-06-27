namespace PersonalWebsite.Core.Models;

public class Post
{
    public string? Id { get; set; }
    public required string Title { get; set; }
    public string? ContentUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageObjectKey { get; set; }
    public required DateTime LastUpdatedUtc { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public required string Slug { get; set; }
}
