using PersonalWebsite.Core.Enums;

namespace PersonalWebsite.Core.Interfaces;

public interface IImageStorage
{
    Task<string> SaveImageAsync(Stream fileStream, string fileName, ImageCategory category);
    Task RemoveImageAsync(string fileName, ImageCategory category);
    string GetImageUrl(string fileName, ImageCategory category);
    string GetImageFileNameFromUrl(string imageUrl);
}
