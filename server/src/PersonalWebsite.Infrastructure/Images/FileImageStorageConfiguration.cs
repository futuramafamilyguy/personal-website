namespace PersonalWebsite.Infrastructure.Images;

public class FileImageStorageConfiguration
{
    public required string BaseImageUrl { get; set; }
    public required string RootImageDirectoryPath { get; set; }
    public required string PictureImageSubDirectory { get; set; }
    public required IEnumerable<string> AllowedImageExtensions { get; set; }
}
