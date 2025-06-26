using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.Cdn;

public class CdnImageStorageDecorator : IImageStorage
{
    private readonly IImageStorage _inner;
    private readonly string _host;

    public CdnImageStorageDecorator(IImageStorage inner, string host)
    {
        _inner = inner;
        _host = host;
    }

    public async Task DeleteObjectAsync(string objectKey) =>
        await _inner.DeleteObjectAsync(objectKey);

    public async Task<string> GeneratePresignedUploadUrlAsync(
        string objectKey,
        TimeSpan expiration
    ) => await _inner.GeneratePresignedUploadUrlAsync(objectKey, expiration);

    public string GetPublicUrl(string objectKey) => $"{_host}/{objectKey}";
}
