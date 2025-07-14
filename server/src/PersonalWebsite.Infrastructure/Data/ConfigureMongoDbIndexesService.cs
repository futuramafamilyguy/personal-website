using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Infrastructure.Data.Movies;

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
        var collection = database.GetCollection<MovieDocument>(_settings.MoviesCollectionName);

        var yearIndex = Builders<MovieDocument>.IndexKeys.Ascending(movie => movie.Year);
        var indexOptions = new CreateIndexOptions { Name = "year" };
        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<MovieDocument>(yearIndex, indexOptions),
            cancellationToken: cancellationToken
        );
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
