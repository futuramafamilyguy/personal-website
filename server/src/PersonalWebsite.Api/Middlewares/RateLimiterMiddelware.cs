using System.Collections.Concurrent;

namespace PersonalWebsite.Api.Middlewares;

public class RateLimiterMiddelware
{
    private const int MAX_QUOTA = 5;
    private const int INTERVAL_MINS = 1;

    private readonly RequestDelegate _next;
    private ConcurrentDictionary<string, Quota> _quotas;

    public RateLimiterMiddelware(RequestDelegate next)
    {
        _next = next;
        _quotas = new ConcurrentDictionary<string, Quota>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        }

        if (clientIp is not null && _quotas.ContainsKey(clientIp))
        {
            var quota = _quotas[clientIp];

            CleanQuota(quota.Timestamps);
            if (quota.Timestamps.Count is MAX_QUOTA)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("fk off");
                return;
            }
            else
            {
                lock (quota.LockObj)
                {
                    quota.Timestamps.Enqueue(DateTime.UtcNow);
                    Console.WriteLine($"#{quota.Timestamps.Count} @ {DateTime.UtcNow}");
                }
            }
        }
        else
        {
            var quota = new Quota();
            quota.Timestamps.Enqueue(DateTime.UtcNow);
            _quotas[clientIp] = quota;
        }

        await _next(context);
    }

    private void CleanQuota(Queue<DateTime> queue)
    {
        while (queue.Count > 0 && DateTime.UtcNow > queue.Peek().AddMinutes(INTERVAL_MINS))
        {
            var t = queue.Dequeue();
            Console.WriteLine($"removed {t}");
        }
    }

    class Quota
    {
        public Queue<DateTime> Timestamps { get; } = new();
        public object LockObj { get; } = new();
    }
}
