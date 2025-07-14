namespace PersonalWebsite.Infrastructure.ImageStorage;

public class ImageStorageConfiguration
{
    public required string Provider { get; set; }
    public required string Host { get; set; }
    public required string BasePathMovie { get; set; }
    public required string BasePathPost { get; set; }
    public required IEnumerable<string> AllowedImageExtensions { get; set; }
    public required bool CdnEnabled { get; set; }
}
