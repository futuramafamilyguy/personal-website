using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.ImageStorage;

public class S3ImageStorage : IImageStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Configuration _s3configuration;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;
    private readonly ILogger<S3ImageStorage> _logger;

    public S3ImageStorage(
        IAmazonS3 s3Client,
        IOptions<S3Configuration> s3configuration,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration,
        ILogger<S3ImageStorage> logger
    )
    {
        _s3Client = s3Client;
        _s3configuration = s3configuration.Value;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
        _logger = logger;
    }

    public Task DeleteObjectAsync(string objectKey) =>
        _s3Client.DeleteObjectAsync(_s3configuration.BucketName, objectKey);

    public async Task<string> GeneratePresignedUploadUrlAsync(string objectKey, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3configuration.BucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiration)
        };
        request.Metadata.Add("Content-Type", GetImageMimeType(objectKey));

        return await _s3Client.GetPreSignedURLAsync(request);
    }

    public string GetPublicUrl(string objectKey) =>
        $"{_imageStorageConfiguration.Host}/{_s3configuration.BucketName}/{objectKey}";

    private static string GetImageMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        extension = extension is "jpg" ? "jpeg" : extension;

        return $"image/{extension}";
    }
}
