using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Data;
using PersonalWebsite.Infrastructure.Data.Cinemas;
using PersonalWebsite.Infrastructure.Data.Movies;
using PersonalWebsite.Infrastructure.Data.Posts;
using PersonalWebsite.Infrastructure.Data.Visits;
using PersonalWebsite.Infrastructure.ImageStorage;
using PersonalWebsite.Infrastructure.MarkdownStorage;

namespace PersonalWebsite.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // repositories are registered as singletons since database interactions are not user-specific
        services.AddSingleton<VisitStatisticsRepository>();
        services.AddSingleton<IMovieRepository, MovieRepository>();
        services.AddSingleton<ICinemaRepository, CinemaRepository>();
        services.AddSingleton<IPostRepository, PostRepository>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddImageStorageServices();
        services.AddMarkdownStorageServices();

        return services;
    }

    public static IServiceCollection AddAmazonS3Services(
        this IServiceCollection services,
        AWSOptions options
    )
    {
        services.AddDefaultAWSOptions(options);
        services.AddAWSService<IAmazonS3>();

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

    private static void AddImageStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<S3ImageStorage>();
        services.AddSingleton<ImageStorageFactory>();

        services.AddSingleton<IImageStorage>(provider =>
        {
            var factory = provider.GetRequiredService<ImageStorageFactory>();

            return factory.CreateImageStorage();
        });
    }

    private static void AddMarkdownStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<S3MarkdownStorage>();
        services.AddSingleton<MarkdownStorageFactory>();

        services.AddSingleton<IMarkdownStorage>(provider =>
        {
            var factory = provider.GetRequiredService<MarkdownStorageFactory>();

            return factory.CreateMarkdownStorage();
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
