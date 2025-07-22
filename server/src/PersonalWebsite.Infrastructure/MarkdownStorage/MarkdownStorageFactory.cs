using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageFactory
{
    private readonly S3MarkdownStorage _s3MarkdownStorage;
    private readonly MarkdownStorageConfiguration _markdownStorageConfiguration;
    private readonly CdnConfiguration _cdnConfiguration;

    public MarkdownStorageFactory(
        IOptions<MarkdownStorageConfiguration> markdownStorageConfiguration,
        IOptions<CdnConfiguration> cdnConfiguration,
        S3MarkdownStorage s3MarkdownStorage
    )
    {
        _s3MarkdownStorage = s3MarkdownStorage;
        _markdownStorageConfiguration = markdownStorageConfiguration.Value;
        _cdnConfiguration = cdnConfiguration.Value;
    }

    public IMarkdownStorage CreateMarkdownStorage()
    {
        var storageProvider = _markdownStorageConfiguration.Provider;

        var markdownStorage = storageProvider switch
        {
            "S3" => _s3MarkdownStorage,
            _ => throw new InvalidOperationException("markdown storage provider not supported")
        };

        if (_markdownStorageConfiguration.CdnEnabled)
            return new CdnMarkdownStorageDecorator(markdownStorage, _cdnConfiguration.Host);

        return markdownStorage;
    }
}
