﻿using Microsoft.Extensions.Options;
using PersonalWebsite.Api.VisitTracking;
using PersonalWebsite.Infrastructure.Data.Visits;

namespace PersonalWebsite.Api.Middlewares;

public class VisitTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly VisitorService _visitorService;
    private readonly VisitExclusionConfiguration _configuration;

    public VisitTrackingMiddleware(
        RequestDelegate next,
        VisitorService visitorService,
        IOptions<VisitExclusionConfiguration> configuration
    )
    {
        _next = next;
        _visitorService = visitorService;
        _configuration = configuration.Value;
    }

    public async Task InvokeAsync(
        HttpContext context,
        VisitStatisticsRepository _visitStatisticsRepository
    )
    {
        var requestIp = context.Items["ClientIp"]?.ToString();

        if (
            !context.Session.Keys.Contains("Visited")
            && !ExcludeVisit(context)
            && (
                IsIncrementEndpoint(context.Request.Path)
                || (
                    context.Request.Method == HttpMethods.Get
                    && !IsPathExluded(context.Request.Path)
                )
            )
            && !_visitorService.IsWebCrawler(requestIp)
            && !_visitorService.IsRecentVisitor(requestIp)
        )
        {
            context.Session.SetString("Visited", "true");
            await _visitStatisticsRepository.IncrementVisitAsync(DateTime.UtcNow);
        }

        await _next(context);
    }

    private bool ExcludeVisit(HttpContext context) =>
        context.Request.Cookies.TryGetValue("ExcludeVisit", out var cookieValue)
        && cookieValue == _configuration.ExcludeVisitCookieValue;

    private bool IsPathExluded(PathString path) =>
        path.HasValue
        && !path.Value.Contains("/stats/increment")
        && (
            path.Value.Contains("/favicon.ico")
            || path.Value.Contains("/active-years")
            || path.Value.Contains("/cinemas")
            || path.Value.Contains("/stats")
        );

    private bool IsIncrementEndpoint(PathString path) =>
        path.HasValue && path.Value.Contains("/stats/increment");
}
