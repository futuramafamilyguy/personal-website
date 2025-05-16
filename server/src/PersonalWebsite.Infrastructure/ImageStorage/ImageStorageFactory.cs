using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Cdn;
using PersonalWebsite.Infrastructure.Images.AmazonS3;
using PersonalWebsite.Infrastructure.Images.LocalFileSystem;

namespace PersonalWebsite.Infrastructure.ImageStorage;

public class ImageStorageFactory
{
    private readonly ICdnUrlService _cdnUrlService;
    private readonly ImageStorageConfiguration _configuration;

    private readonly LocalImageStorage _localImageStorage;
    private readonly AmazonS3ImageStorage _amazonS3ImageStorage;

    public ImageStorageFactory(
        ICdnUrlService cdnUrlService,
        IOptions<ImageStorageConfiguration> configuration,
        LocalImageStorage localImageStorage,
        AmazonS3ImageStorage amazonS3ImageStorage
    )
    {
        _cdnUrlService = cdnUrlService;
        _configuration = configuration.Value;
        _localImageStorage = localImageStorage;
        _amazonS3ImageStorage = amazonS3ImageStorage;
    }

    public IImageStorage CreateImageStorage()
    {
        var storageType = _configuration.ImageStorageType;

        var imageStorage = storageType switch
        {
            "Local" => (IImageStorage)_localImageStorage,
            "S3" => _amazonS3ImageStorage,
            _ => throw new InvalidOperationException("Image storage type not supported")
        };

        if (_configuration.CdnEnabled)
            return new CdnImageStorageDecorator(
                imageStorage,
                _cdnUrlService,
                storageType
            );

        return imageStorage;
    }
}
