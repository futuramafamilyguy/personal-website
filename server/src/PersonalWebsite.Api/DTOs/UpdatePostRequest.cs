namespace PersonalWebsite.Api.DTOs;

public class UpdatePostRequest
{
    public required string Title { get; set; }
    public string? MarkdownUrl { get; set; }
    public string? MarkdownObjectKey { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageObjectKey { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public required int MarkdownVersion { get; set; }
}
