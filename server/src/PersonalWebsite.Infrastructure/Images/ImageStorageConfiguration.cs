namespace PersonalWebsite.Infrastructure.Images;

public class ImageStorageConfiguration
{
    public required string BaseImageUrl { get; set; }
    public required string PictureImageDirectory { get; set; }
    public required IEnumerable<string> AllowedImageExtensions { get; set; }
}
