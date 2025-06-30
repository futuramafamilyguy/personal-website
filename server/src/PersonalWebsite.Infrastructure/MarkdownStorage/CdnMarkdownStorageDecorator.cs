using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class CdnMarkdownStorageDecorator : IMarkdownStorage
{
    private readonly IMarkdownStorage _inner;
    private readonly string _host;

    public CdnMarkdownStorageDecorator(IMarkdownStorage inner, string host)
    {
        _inner = inner;
        _host = host;
    }

    public async Task ArchiveObjectAsync(string objectKey) =>
        await _inner.ArchiveObjectAsync(objectKey);

    public async Task<string> GeneratePresignedUploadUrlAsync(
        string objectKey,
        TimeSpan expiration
    ) => await _inner.GeneratePresignedUploadUrlAsync(objectKey, expiration);

    public string GetPublicUrl(string objectKey) => $"{_host}/{objectKey}";
}
