using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Infrastructure.Data.Pictures;

namespace PersonalWebsite.Infrastructure.Data;

public class ConfigureMongoDbIndexesService : IHostedService
{
    private readonly IMongoClient _client;
    private readonly MongoDbConfiguration _settings;

    public ConfigureMongoDbIndexesService(
        IMongoClient client,
        IOptions<MongoDbConfiguration> settings
    )
    {
        _client = client;
        _settings = settings.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var database = _client.GetDatabase(_settings.DatabaseName);
        var collection = database.GetCollection<PictureDocument>(_settings.PicturesCollectionName);

        var yearIndex = Builders<PictureDocument>.IndexKeys.Ascending(picture =>
            picture.YearWatched
        );
        var indexOptions = new CreateIndexOptions { Name = "pictures_year_watched" };
        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<PictureDocument>(yearIndex, indexOptions),
            cancellationToken: cancellationToken
        );
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
