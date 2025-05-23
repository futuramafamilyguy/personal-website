﻿using Microsoft.Extensions.Options;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Cdn;

namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageFactory
{
    private readonly ICdnUrlService _cdnUrlService;
    private readonly MarkdownStorageConfiguration _configuration;

    private readonly AmazonS3MarkdownStorage _amazonS3MarkdownStorage;

    public MarkdownStorageFactory(
        ICdnUrlService cdnUrlService,
        IOptions<MarkdownStorageConfiguration> configuration,
        AmazonS3MarkdownStorage amazonS3MarkdownStorage
    )
    {
        _cdnUrlService = cdnUrlService;
        _configuration = configuration.Value;
        _amazonS3MarkdownStorage = amazonS3MarkdownStorage;
    }

    public IMarkdownStorage CreateMarkdownStorage()
    {
        var storageProvider = _configuration.Provider;

        var markdownStorage = storageProvider switch
        {
            "S3" => _amazonS3MarkdownStorage,
            _ => throw new InvalidOperationException("Markdown storage provider not supported")
        };

        if (_configuration.CdnEnabled)
            return new CdnMarkdownStorageDecorator(
                markdownStorage,
                _cdnUrlService,
                storageProvider
            );

        return markdownStorage;
    }
}
