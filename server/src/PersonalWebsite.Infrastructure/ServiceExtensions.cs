using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.Data.Pictures;

namespace PersonalWebsite.Infrastructure;

public static class ServiceExtensions
{
    public static void AddMongoClient(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPictureRepository, PictureRepository>();

        return services;
    }
}
