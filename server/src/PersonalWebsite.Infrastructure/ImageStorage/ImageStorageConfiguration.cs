namespace PersonalWebsite.Infrastructure.ImageStorage;

public class ImageStorageConfiguration
{
    public required string ImageStorageType { get; set; }
    public required string BaseImageUrl { get; set; }
    public required string PictureImageDirectory { get; set; }
    public required string PostImageDirectory { get; set; }
    public required IEnumerable<string> AllowedImageExtensions { get; set; }
    public required bool CdnEnabled { get; set; }
}
