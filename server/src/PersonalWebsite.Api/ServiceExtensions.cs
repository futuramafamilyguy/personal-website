using PersonalWebsite.Api.VisitTracking;

namespace PersonalWebsite.Api;

public static class ServiceExtensions
{
    public static IServiceCollection AddVisitTrackingServices(this IServiceCollection services)
    {
        services.AddSingleton(new VisitorService(TimeSpan.FromDays(1)));

        return services;
    }
}
