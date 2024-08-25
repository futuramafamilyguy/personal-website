using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Data;
using PersonalWebsite.Infrastructure.Data.Cinemas;
using PersonalWebsite.Infrastructure.Data.Pictures;
using PersonalWebsite.Infrastructure.Data.Posts;
using PersonalWebsite.Infrastructure.Data.Visits;
using PersonalWebsite.Infrastructure.Images.AmazonS3;
using PersonalWebsite.Infrastructure.Images.LocalFileSystem;
using PersonalWebsite.Infrastructure.ImageStorage;
using PersonalWebsite.Infrastructure.MarkdownStorage;

namespace PersonalWebsite.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<VisitStatisticsRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<ICinemaRepository, CinemaRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
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
        services.AddScoped<LocalImageStorage>();
        services.AddScoped<AmazonS3ImageStorage>();
        services.AddSingleton<ImageStorageFactory>();

        services.AddScoped(provider =>
        {
            var factory = provider.GetRequiredService<ImageStorageFactory>();

            return factory.CreateImageStorage();
        });
    }

    private static void AddMarkdownStorageServices(this IServiceCollection services)
    {
        services.AddScoped<AmazonS3MarkdownStorage>();
        services.AddSingleton<MarkdownStorageFactory>();

        services.AddScoped(provider =>
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
