using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Core.Interfaces;

public interface IImageStorage
{
    Task<string> SaveImageAsync(Stream fileStream, string fileName, string directory);
    Task RemoveImageAsync(string fileName, string directory);
    string GetImageFileNameFromUrl(string imageUrl);
    bool IsValidImageFormat(string fileName);
}
