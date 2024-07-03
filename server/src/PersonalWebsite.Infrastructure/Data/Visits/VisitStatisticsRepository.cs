using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PersonalWebsite.Infrastructure.Data.Visits;

public class VisitStatisticsRepository
{
    private readonly IMongoCollection<VisitStatistics> _visitStatistics;
    private readonly MongoDbConfiguration _settings;

    public VisitStatisticsRepository(IMongoClient client, IOptions<MongoDbConfiguration> settings)
    {
        _settings = settings.Value;
        var database = client.GetDatabase(_settings.DatabaseName);
        _visitStatistics = database.GetCollection<VisitStatistics>(
            _settings.VisitStatisticsCollectionName
        );
    }

    public async Task<VisitStatistics?> GetVisitStatisticsAsync()
    {
        var stats = await _visitStatistics.Find(_ => true).FirstOrDefaultAsync();

        return stats;
    }

    public async Task IncrementVisitAsync(DateTime visitedAtUtc)
    {
        var update = Builders<VisitStatistics>
            .Update.Inc(s => s.TotalVisits, 1)
            .Set(s => s.LatestVisitUtc, visitedAtUtc);

        await _visitStatistics.UpdateOneAsync(
            _ => true,
            update,
            new UpdateOptions { IsUpsert = true }
        );
    }
}
