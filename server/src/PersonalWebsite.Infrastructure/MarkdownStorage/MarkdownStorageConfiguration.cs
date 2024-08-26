namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageConfiguration
{
    public required string MarkdownStorageType { get; set; }
    public required string BaseUrl { get; set; }
    public required string PostMarkdownDirectory { get; set; }
}
