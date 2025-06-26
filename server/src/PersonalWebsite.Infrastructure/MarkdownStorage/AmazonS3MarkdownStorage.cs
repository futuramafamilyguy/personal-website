using System.Text;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class AmazonS3MarkdownStorage : IMarkdownStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Configuration _s3configuration;
    private readonly MarkdownStorageConfiguration _markdownStorageConfiguration;
    private readonly ILogger<AmazonS3MarkdownStorage> _logger;

    public AmazonS3MarkdownStorage(
        IAmazonS3 s3Client,
        IOptions<S3Configuration> s3configuration,
        IOptions<MarkdownStorageConfiguration> markdownStorageConfiguration,
        ILogger<AmazonS3MarkdownStorage> logger
    )
    {
        _s3Client = s3Client;
        _s3configuration = s3configuration.Value;
        _markdownStorageConfiguration = markdownStorageConfiguration.Value;
        _logger = logger;
    }

    public async Task<string> CopyMarkdownAsync(
        string fileName,
        string newFileName,
        string basePath
    )
    {
        try
        {
            var key = $"{basePath}/{fileName}";
            var newKey = $"{basePath}/{newFileName}";

            var request = new CopyObjectRequest
            {
                SourceBucket = _s3configuration.BucketName,
                SourceKey = key,
                DestinationBucket = _s3configuration.BucketName,
                DestinationKey = newKey
            };

            await _s3Client.CopyObjectAsync(request);
            _logger.LogInformation(
                $"Successfully renamed markdown '{fileName}' to '{newFileName}' at '{basePath}'"
            );

            var markdownUrl =
                $"{_markdownStorageConfiguration.Host}/{_s3configuration.BucketName}/{basePath}/{newFileName}";

            return markdownUrl;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(
                ex,
                $"AWS S3 error encountered when renaming markdown '{fileName}' to '{newFileName}' from '{basePath}'"
            );
            throw new StorageException("Failed to rename markdown due to AWS S3 error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"Unexpected error encountered when renaming markdown '{fileName}' to '{newFileName}' from '{basePath}'"
            );
            throw new StorageException("Failed to rename markdown due to unexpected error", ex);
        }
    }

    public string GetMarkdownFileNameFromUrl(string postUrl)
    {
        var uri = new Uri(postUrl);
        var fileName = Path.GetFileName(uri.LocalPath);

        return fileName;
    }

    public async Task RemoveMarkdownAsync(string fileName, string basePath)
    {
        var key = $"{basePath}/{fileName}";

        try
        {
            // DeleteObjectAsync doesn't throw any errors if the key does not exist so have to call GetObjectAsync
            await _s3Client.GetObjectAsync(_s3configuration.BucketName, key);

            await _s3Client.DeleteObjectAsync(_s3configuration.BucketName, key);
            _logger.LogInformation($"Successfully deleted markdown '{fileName}' from '{basePath}'");
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(
                ex,
                $"AWS S3 error encountered when deleting markdown '{fileName}' from '{basePath}'"
            );
            throw new StorageException("Failed to delete markdown due to AWS S3 error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"Unexpected error encountered when deleting markdown '{fileName}' from '{basePath}'"
            );
            throw new StorageException("Failed to delete markdown due to unexpected error", ex);
        }
    }

    public async Task<string> SaveMarkdownAsync(string content, string fileName, string basePath)
    {
        try
        {
            var byteArray = Encoding.UTF8.GetBytes(content);
            using var stream = new MemoryStream(byteArray);
            var request = new PutObjectRequest
            {
                BucketName = _s3configuration.BucketName,
                Key = $"{basePath}/{fileName}",
                InputStream = stream,
                ContentType = "text/markdown"
            };

            await _s3Client.PutObjectAsync(request);
            _logger.LogInformation($"Successfully uploaded markdown '{fileName}' at '{basePath}'");

            var markdownUrl =
                $"{_markdownStorageConfiguration.Host}/{_s3configuration.BucketName}/{basePath}/{fileName}";

            return markdownUrl;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(
                ex,
                $"AWS S3 error encountered when uploading markdown '{fileName}' to bucket"
            );
            throw new StorageException("Failed to upload markdown due to AWS S3 error", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                $"Unexpected error encountered when uploading markdown '{fileName}'"
            );
            throw new StorageException("Failed to upload markdown due to unexpected error", ex);
        }
    }
}
