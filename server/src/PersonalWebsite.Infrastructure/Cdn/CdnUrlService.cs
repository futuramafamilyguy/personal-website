using Microsoft.Extensions.Options;

namespace PersonalWebsite.Infrastructure.Cdn;

public class CdnUrlService : ICdnUrlService
{
    private readonly CdnConfiguration _cdnConfiguration;
    private readonly AmazonS3Configuration _s3Configuration;

    public CdnUrlService(
        IOptions<CdnConfiguration> cdnConfiguration,
        IOptions<AmazonS3Configuration> s3Configuration
    )
    {
        _cdnConfiguration = cdnConfiguration.Value;
        _s3Configuration = s3Configuration.Value;
    }

    public string ConvertToCdnUrl(string rawUrl, string storageProvider)
    {
        var path = storageProvider switch
        {
            "S3" => GetObjectKeyFromS3Url(_s3Configuration.Bucket, rawUrl),
            _ => throw new InvalidOperationException("Storage provider not supported")
        };

        return $"{_cdnConfiguration.BaseUrl.TrimEnd('/')}/{path}";
    }

    private static string GetObjectKeyFromS3Url(string bucketName, string rawUrl)
    {
        var uri = new Uri(rawUrl);
        var absolutePath = uri.AbsolutePath;
        var trimmedPath = absolutePath.TrimStart('/');
        var objectKey = trimmedPath.Substring(bucketName.Length + 1);

        return objectKey;
    }
}
