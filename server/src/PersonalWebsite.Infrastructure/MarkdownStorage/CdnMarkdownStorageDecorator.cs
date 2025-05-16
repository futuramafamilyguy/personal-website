using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Cdn;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class CdnMarkdownStorageDecorator : IMarkdownStorage
{
    private readonly IMarkdownStorage _inner;
    private readonly ICdnUrlService _cdnUrlService;
    private readonly string _innerStorageProvider;

    public CdnMarkdownStorageDecorator(
        IMarkdownStorage inner,
        ICdnUrlService cdnUrlService,
        string innerStorageProvider
    )
    {
        _inner = inner;
        _cdnUrlService = cdnUrlService;
        _innerStorageProvider = innerStorageProvider;
    }

    public async Task<string> CopyMarkdownAsync(
        string fileName,
        string newFileName,
        string basePath
    ) => await _inner.CopyMarkdownAsync(fileName, newFileName, basePath);

    public string GetMarkdownFileNameFromUrl(string postUrl) =>
        _inner.GetMarkdownFileNameFromUrl(postUrl);

    public async Task RemoveMarkdownAsync(string fileName, string basePath) =>
        await _inner.RemoveMarkdownAsync(fileName, basePath);

    public async Task<string> SaveMarkdownAsync(string content, string fileName, string basePath)
    {
        var rawUrl = await _inner.SaveMarkdownAsync(content, fileName, basePath);
        return _cdnUrlService.ConvertToCdnUrl(rawUrl, _innerStorageProvider);
    }
}
