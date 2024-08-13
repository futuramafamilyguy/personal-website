using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Images.LocalFileSystem;

namespace PersonalWebsite.Infrastructure.Images.AmazonS3;

public class AmazonS3ImageStorage : IImageStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly AmazonS3ImageStorageConfiguration _configuration;
    private readonly ImageStorageConfiguration _baseConfiguration;
    private readonly ILogger<LocalImageStorage> _logger;

    public AmazonS3ImageStorage(
        IAmazonS3 s3Client,
        IOptions<AmazonS3ImageStorageConfiguration> configuration,
        IOptions<ImageStorageConfiguration> baseConfiguration,
        ILogger<LocalImageStorage> logger
    )
    {
        _s3Client = s3Client;
        _configuration = configuration.Value;
        _baseConfiguration = baseConfiguration.Value;
        _logger = logger;
    }

    public string GetImageFileNameFromUrl(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        var fileName = Path.GetFileName(uri.LocalPath);

        return fileName;
    }

    public string GetImageUrl(string fileName, string directory) =>
        $"{_baseConfiguration.BaseImageUrl}/{_configuration.Bucket}/{directory}/{fileName}";

    public async Task RemoveImageAsync(string fileName, string directory)
    {
        var key = $"{directory}/{fileName}";

        try
        {
            // throws AmazonS3Exception if key does not exist but DeleteObjectAsync doesn't throw anything
            await _s3Client.GetObjectAsync(_configuration.Bucket, key);

            await _s3Client.DeleteObjectAsync(_configuration.Bucket, key);
            _logger.LogInformation($"Successfully deleted image '{fileName}' at '{directory}'");
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(
                ex,
                $"Failed to delete image '{fileName}' at '{directory}' (likely because it doesn't exist in the bucket)"
            );
            throw new FileNotFoundException("Image not found", key);
        }
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName, string directory)
    {
        if (!IsValidImageFormat(fileName))
        {
            _logger.LogError($"The image format of {fileName} is not supported");
            throw new InvalidImageFormatException(
                $"The image format of {fileName} is not supported"
            );
        }

        try
        {
            var request = new PutObjectRequest()
            {
                BucketName = _configuration.Bucket,
                Key = $"{directory}/{fileName}",
                InputStream = fileStream
            };
            request.Metadata.Add("Content-Type", GetImageMimeType(fileName));
            await _s3Client.PutObjectAsync(request);
            _logger.LogInformation($"Successfully uploaded image '{fileName}' at '{directory}'");
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Error encountered when uploading image '{fileName}' to bucket");
            throw new ImageStorageException($"Failed to upload image '{fileName}'");
        }

        return fileName;
    }

    private bool IsValidImageFormat(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return _baseConfiguration.AllowedImageExtensions.Contains(extension);
    }

    private static string GetImageMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        extension = extension is "jpg" ? "jpeg" : extension;

        return $"image/{extension}";
    }
}
