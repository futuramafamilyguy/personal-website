using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class S3MarkdownStorage : IMarkdownStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Configuration _s3configuration;
    private readonly MarkdownStorageConfiguration _markdownStorageConfiguration;

    public S3MarkdownStorage(
        IAmazonS3 s3Client,
        IOptions<S3Configuration> s3configuration,
        IOptions<MarkdownStorageConfiguration> markdownStorageConfiguration
    )
    {
        _s3Client = s3Client;
        _s3configuration = s3configuration.Value;
        _markdownStorageConfiguration = markdownStorageConfiguration.Value;
    }

    public async Task ArchiveObjectAsync(string objectKey)
    {
        var archiveKey = objectKey.Replace("posts/", "posts/archive/");
        var request = new CopyObjectRequest
        {
            SourceBucket = _s3configuration.BucketName,
            SourceKey = objectKey,
            DestinationBucket = _s3configuration.BucketName,
            DestinationKey = archiveKey
        };
        await _s3Client.CopyObjectAsync(request);

        await DeleteObjectAsync(objectKey);
    }

    public async Task DeleteObjectAsync(string objectKey) =>
        await _s3Client.DeleteObjectAsync(_s3configuration.BucketName, objectKey);

    public async Task<string> GeneratePresignedUploadUrlAsync(string objectKey, TimeSpan expiration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3configuration.BucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiration),
            ContentType = "text/markdown"
        };

        return await _s3Client.GetPreSignedURLAsync(request);
    }

    public string GetPublicUrl(string objectKey) =>
        $"{_markdownStorageConfiguration.Host}/{_s3configuration.BucketName}/{objectKey}";
}
