using Microsoft.Extensions.Options;
using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Middlewares;

public class VisitTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly VisitExclusionConfiguration _configuration;

    public VisitTrackingMiddleware(
        RequestDelegate next,
        IOptions<VisitExclusionConfiguration> configuration
    )
    {
        _next = next;
        _configuration = configuration.Value;
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
            if (!IsExcludedVisit(context))
            {
                context.Session.SetString("Visited", "true");
                await _visitStatisticsRepository.IncrementVisitAsync(DateTimeOffset.UtcNow);
            }
        }

        await _next(context);
    }

    private bool IsExcludedVisit(HttpContext context) =>
        context.Request.Cookies.TryGetValue("ExcludeVisit", out var cookieValue)
        && cookieValue == _configuration.ExcludeVisitCookieValue;
}
