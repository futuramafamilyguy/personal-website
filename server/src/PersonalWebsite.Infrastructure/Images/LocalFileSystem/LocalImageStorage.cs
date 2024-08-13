using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.Images.LocalFileSystem;

public class LocalImageStorage : IImageStorage
{
    private readonly LocalImageStorageConfiguration _configuration;
    private readonly ImageStorageConfiguration _baseConfiguration;
    private readonly ILogger<LocalImageStorage> _logger;

    public LocalImageStorage(
        IOptions<LocalImageStorageConfiguration> configuration,
        IOptions<ImageStorageConfiguration> baseConfiguration,
        ILogger<LocalImageStorage> logger
    )
    {
        _configuration = configuration.Value;
        _baseConfiguration = baseConfiguration.Value;
        _logger = logger;
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName, string directory)
    {
        if (!IsValidImageFormat(fileName))
        {
            throw new InvalidImageFormatException(
                $"The image format of {fileName} is not supported."
            );
        }

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

    public string GetImageUrl(string fileName, string directory) =>
        $"{_baseConfiguration.BaseImageUrl}/{directory}/{fileName}";

    public string GetImageFileNameFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var fileName = Path.GetFileName(uri.LocalPath);

        return fileName;
    }

    private bool IsValidImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _baseConfiguration.AllowedImageExtensions.Contains(extension);
    }

    private string GetImageFilePath(string fileName, string directory)
    {
        var imagesDirectoryPath = Path.Combine(_configuration.RootImageDirectoryPath, directory);
        var filePath = Path.Combine(imagesDirectoryPath, fileName);

        return filePath;
    }

    private void CreateImageDirectoryIfNotExists(string directory)
    {
        var imagesDirectoryPath = Path.Combine(_configuration.RootImageDirectoryPath, directory);
        if (!Directory.Exists(imagesDirectoryPath))
        {
            Directory.CreateDirectory(imagesDirectoryPath);
            _logger.LogInformation($"Image directory created at {imagesDirectoryPath}");
        }
    }
}
