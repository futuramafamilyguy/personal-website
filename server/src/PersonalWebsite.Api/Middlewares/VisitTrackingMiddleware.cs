using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Middlewares;

public class VisitTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private VisitStatisticsRepository _visitStatisticsRepository;

    public VisitTrackingMiddleware(
        RequestDelegate next,
        VisitStatisticsRepository visitStatisticsRepository
    )
    {
        _next = next;
        _visitStatisticsRepository = visitStatisticsRepository;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Session.Keys.Contains("Visited"))
        {
            context.Session.SetString("Visited", "true");
            await _visitStatisticsRepository.IncrementVisitAsync(DateTimeOffset.UtcNow);
        }

        await _next(context);
    }
}
