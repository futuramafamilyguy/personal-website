using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Cdn;

namespace PersonalWebsite.Infrastructure.ImageStorage;

public class ImageStorageFactory
{
    private readonly S3ImageStorage _s3ImageStorage;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;
    private readonly CdnConfiguration _cdnConfiguration;

    public ImageStorageFactory(
        IOptions<ImageStorageConfiguration> imageStorageConfiguration,
        IOptions<CdnConfiguration> cdnConfiguration,
        S3ImageStorage s3ImageStorage
    )
    {
        _s3ImageStorage = s3ImageStorage;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
        _cdnConfiguration = cdnConfiguration.Value;
    }

    public IImageStorage CreateImageStorage()
    {
        var storageProvider = _imageStorageConfiguration.Provider;

        var imageStorage = storageProvider switch
        {
            "S3" => _s3ImageStorage,
            _ => throw new InvalidOperationException("image storage provider not supported")
        };

        if (_imageStorageConfiguration.CdnEnabled)
            return new CdnImageStorageDecorator(imageStorage, _cdnConfiguration.Host);

        return imageStorage;
    }
}
