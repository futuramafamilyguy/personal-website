using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.Images;

public class FileImageStorage : IImageStorage
{
    private readonly FileImageStorageConfiguration _configuration;
    private readonly ILogger<FileImageStorage> _logger;

    public FileImageStorage(
        IOptions<FileImageStorageConfiguration> configuration,
        ILogger<FileImageStorage> logger
    )
    {
        _configuration = configuration.Value;
        _logger = logger;
    }

    public async Task<string> SaveImageAsync(
        Stream fileStream,
        string fileName,
        ImageCategory category
    )
    {
        if (!IsValidImageFormat(fileName))
        {
            throw new InvalidImageFormatException(
                $"The image format of {fileName} is not supported."
            );
        }

        try
        {
            CreateImageDirectoryIfNotExists(category);
            var filePath = GetImageFilePath(fileName, category);

            using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream);
            _logger.LogInformation($"{category} image created at {filePath}");

            return fileName;
        }
        catch (IOException ex)
        {
            throw new ImageStorageException("An error occurred while saving the image.", ex);
        }
    }

    public async Task RemoveImageAsync(string fileName, ImageCategory category)
    {
        var filePath = GetImageFilePath(fileName, category);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Image {filePath} not found");
        }
        await Task.Run(() => File.Delete(filePath));
        _logger.LogInformation($"{category} image deleted at {filePath}");
    }

    public string GetImageUrl(string fileName, ImageCategory category) =>
        $"{_configuration.BaseImageUrl}/{GetSubDirectory(category)}/{fileName}";

    public string GetImageFileNameFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var fileName = Path.GetFileName(uri.LocalPath);

        return fileName;
    }

    private bool IsValidImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _configuration.AllowedImageExtensions.Contains(extension);
    }

    private string GetSubDirectory(ImageCategory category)
    {
        switch (category)
        {
            case ImageCategory.Picture:
                return _configuration.PictureImageSubDirectory;
            default:
                throw new ArgumentException("Image category not supported");
        }
    }

    private string GetImageFilePath(string fileName, ImageCategory category)
    {
        var imagesDirectoryPath = Path.Combine(
            _configuration.RootImageDirectoryPath,
            GetSubDirectory(category)
        );
        var filePath = Path.Combine(imagesDirectoryPath, fileName);

        return filePath;
    }

    private void CreateImageDirectoryIfNotExists(ImageCategory category)
    {
        var imagesDirectoryPath = Path.Combine(
            _configuration.RootImageDirectoryPath,
            GetSubDirectory(category)
        );
        if (!Directory.Exists(imagesDirectoryPath))
        {
            Directory.CreateDirectory(imagesDirectoryPath);
            _logger.LogInformation(
                $"Image directory for {category} created at {imagesDirectoryPath}"
            );
        }
    }
}
