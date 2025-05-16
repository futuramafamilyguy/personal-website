using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Cdn;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MarkdownStorageConfiguration _configuration;

    public MarkdownStorageFactory(
        IServiceProvider serviceProvider,
        IOptions<MarkdownStorageConfiguration> configuration
    )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration.Value;
    }

    public IMarkdownStorage CreateMarkdownStorage()
    {
        var storageType = _configuration.MarkdownStorageType;

        using var scope = _serviceProvider.CreateScope();
        var storage = storageType switch
        {
            "S3" => scope.ServiceProvider.GetRequiredService<AmazonS3MarkdownStorage>(),
            _ => throw new InvalidOperationException("Markdown storage type not supported")
        };

        if (_configuration.CdnEnabled)
            return new CdnMarkdownStorageDecorator(
                storage,
                scope.ServiceProvider.GetRequiredService<ICdnUrlService>(),
                storageType
            );

        return storage;
    }
}
