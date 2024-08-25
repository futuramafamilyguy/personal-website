using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Images.AmazonS3;
using PersonalWebsite.Infrastructure.Images.LocalFileSystem;

namespace PersonalWebsite.Infrastructure.ImageStorage;

public class ImageStorageFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ImageStorageConfiguration _configuration;

    public ImageStorageFactory(
        IServiceProvider serviceProvider,
        IOptions<ImageStorageConfiguration> configuration
    )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration.Value;
    }

    public IImageStorage CreateImageStorage()
    {
        var storageType = _configuration.ImageStorageType;

        using var scope = _serviceProvider.CreateScope();
        return storageType switch
        {
            "Local" => scope.ServiceProvider.GetRequiredService<LocalImageStorage>(),
            "S3" => scope.ServiceProvider.GetRequiredService<AmazonS3ImageStorage>(),
            _ => throw new InvalidOperationException("Image storage type not supported")
        };
    }
}
