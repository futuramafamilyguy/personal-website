using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<ICinemaService, CinemaService>();
        services.AddScoped<IPictureCinemaOrchestrator, PictureCinemaOrchestrator>();
        services.AddScoped<IPostService, PostService>();

        return services;
    }
}
