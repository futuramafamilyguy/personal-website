namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageConfiguration
{
    public required string Provider { get; set; }
    public required string BaseUrl { get; set; }
    public required string BasePath { get; set; }
    public required bool CdnEnabled { get; set; }
}
