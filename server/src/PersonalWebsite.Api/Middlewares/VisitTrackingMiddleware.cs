using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Middlewares;

public class VisitTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public VisitTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        VisitStatisticsRepository _visitStatisticsRepository
    )
    {
        if (
            !context.Session.Keys.Contains("Visited")
            && context.Request.Path.HasValue
            && !context.Request.Path.Value.Contains("/favicon.ico")
        )
        {
            context.Session.SetString("Visited", "true");
            await _visitStatisticsRepository.IncrementVisitAsync(DateTimeOffset.UtcNow);
        }

        await _next(context);
    }
}
