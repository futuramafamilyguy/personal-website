using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.Cdn;

public class CdnImageStorageDecorator : IImageStorage
{
    private readonly IImageStorage _inner;
    private readonly ICdnUrlService _cdnUrlService;
    private readonly string _innerStorageProvider;

    public CdnImageStorageDecorator(
        IImageStorage inner,
        ICdnUrlService cdnUrlService,
        string innerStorageProvider
    )
    {
        _inner = inner;
        _cdnUrlService = cdnUrlService;
        _innerStorageProvider = innerStorageProvider;
    }

    public string GetImageFileNameFromUrl(string imageUrl) =>
        _inner.GetImageFileNameFromUrl(imageUrl);

    public bool IsValidImageFormat(string fileName) => _inner.IsValidImageFormat(fileName);

    public Task RemoveImageAsync(string fileName, string directory) =>
        _inner.RemoveImageAsync(fileName, directory);

    public async Task<string> SaveImageAsync(Stream fileStream, string fileName, string directory)
    {
        var rawUrl = await _inner.SaveImageAsync(fileStream, fileName, directory);
        return _cdnUrlService.ConvertToCdnUrl(rawUrl, _innerStorageProvider);
    }
}
