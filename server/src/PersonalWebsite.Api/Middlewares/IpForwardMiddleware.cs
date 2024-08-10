namespace PersonalWebsite.Api.Middlewares;

public class IpForwardMiddleware
{
    private readonly RequestDelegate _next;

    public IpForwardMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        }

        context.Items["ClientIp"] = clientIp;

        await _next(context);
    }
}
