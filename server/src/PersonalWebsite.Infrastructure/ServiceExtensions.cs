using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace PersonalWebsite.Infrastructure;

public static class ServiceExtensions
{
    public static void AddMongoClient(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
    }
}
