using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageFactory
{
    private readonly MarkdownStorageConfiguration _configuration;

    private readonly S3MarkdownStorage _s3MarkdownStorage;

    public MarkdownStorageFactory(
        IOptions<MarkdownStorageConfiguration> configuration,
        S3MarkdownStorage s3MarkdownStorage
    )
    {
        _configuration = configuration.Value;
        _s3MarkdownStorage = s3MarkdownStorage;
    }

    public IMarkdownStorage CreateMarkdownStorage()
    {
        var storageProvider = _configuration.Provider;

        var markdownStorage = storageProvider switch
        {
            "S3" => _s3MarkdownStorage,
            _ => throw new InvalidOperationException("Markdown storage provider not supported")
        };

        if (_configuration.CdnEnabled)
            return new CdnMarkdownStorageDecorator(markdownStorage, storageProvider);

        return markdownStorage;
    }
}
