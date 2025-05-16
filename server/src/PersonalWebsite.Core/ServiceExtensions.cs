using Microsoft.Extensions.DependencyInjection;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Services;

namespace PersonalWebsite.Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IPictureService, PictureService>();
        services.AddSingleton<ICinemaService, CinemaService>();
        services.AddSingleton<IPictureCinemaOrchestrator, PictureCinemaOrchestrator>();
        services.AddSingleton<IPostService, PostService>();

        return services;
    }
}
