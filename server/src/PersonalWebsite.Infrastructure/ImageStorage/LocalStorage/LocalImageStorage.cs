using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.ImageStorage;

namespace PersonalWebsite.Infrastructure.Images.LocalFileSystem;

public class LocalImageStorage : IImageStorage
{
    private const string Wwwroot = "wwwroot";

    private readonly ImageStorageConfiguration _baseConfiguration;
    private readonly ILogger<LocalImageStorage> _logger;

    public LocalImageStorage(
        IOptions<ImageStorageConfiguration> baseConfiguration,
        ILogger<LocalImageStorage> logger
    )
    {
        _baseConfiguration = baseConfiguration.Value;
        _logger = logger;
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName, string directory)
    {
        try
        {
            CreateImageDirectoryIfNotExists(directory);
            var filePath = GetImageFilePath(fileName, directory);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);
            _logger.LogInformation($"Image created at {filePath}");

            return fileName;
        }
        catch (IOException ex)
        {
            throw new ImageStorageException("An error occurred while saving the image.", ex);
        }
    }

    public async Task RemoveImageAsync(string fileName, string directory)
    {
        var filePath = GetImageFilePath(fileName, directory);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Image {filePath} not found");
        }
        await Task.Run(() => File.Delete(filePath));
        _logger.LogInformation($"Image deleted at {filePath}");
    }

    public string GetImageFileNameFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var fileName = Path.GetFileName(uri.LocalPath);

        return fileName;
    }

    public bool IsValidImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _baseConfiguration.AllowedImageExtensions.Contains(extension);
    }

    private static string ConvertImageUrlToLocalPath(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var pathAndQuery = uri.PathAndQuery;

        return Wwwroot.TrimEnd('/') + pathAndQuery;
    }

    private string GetImageFilePath(string fileName, string directory)
    {
        var imagesDirectoryPath = Path.Combine(
            ConvertImageUrlToLocalPath(_baseConfiguration.BaseImageUrl),
            directory
        );
        var filePath = Path.Combine(imagesDirectoryPath, fileName);

        return filePath;
    }

    private void CreateImageDirectoryIfNotExists(string directory)
    {
        var imagesDirectoryPath = Path.Combine(
            ConvertImageUrlToLocalPath(_baseConfiguration.BaseImageUrl),
            directory
        );
        if (!Directory.Exists(imagesDirectoryPath))
        {
            Directory.CreateDirectory(imagesDirectoryPath);
            _logger.LogInformation($"Image directory created at {imagesDirectoryPath}");
        }
    }
}
