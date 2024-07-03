using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Data;
using PersonalWebsite.Infrastructure.Data.Cinemas;
using PersonalWebsite.Infrastructure.Data.Pictures;
using PersonalWebsite.Infrastructure.Data.Visits;
using PersonalWebsite.Infrastructure.Images;

namespace PersonalWebsite.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<VisitStatisticsRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<ICinemaRepository, CinemaRepository>();
        services.AddScoped<IImageStorage, FileImageStorage>();

        return services;
    }

    public static void AddMongoClient(
        this IServiceCollection services,
        string connectionString,
        MongoDbConfiguration mongodbConfig
    )
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var client = new MongoClient(connectionString);
            SeedInitialData(client, mongodbConfig);

            return client;
        });
    }

    private static void SeedInitialData(IMongoClient client, MongoDbConfiguration mongodbConfig)
    {
        var database = client.GetDatabase(mongodbConfig.DatabaseName);
        var collection = database.GetCollection<VisitStatistics>(
            mongodbConfig.VisitStatisticsCollectionName
        );

        var existingDocument = collection.Find(_ => true).FirstOrDefault();
        if (existingDocument == null)
        {
            var startDateDocument = new VisitStatistics
            {
                TotalVisits = 0,
                TrackingStartUtc = DateTime.UtcNow
            };
            collection.InsertOne(startDateDocument);
        }
    }
}
