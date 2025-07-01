using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonalWebsite.Core.Exceptions;

namespace PersonalWebsite.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case DomainValidationException ex:
                context.Result = new ObjectResult(ex.Message) { StatusCode = 400 };
                break;

            case EntityNotFoundException ex:
                context.Result = new ObjectResult(ex.Message) { StatusCode = 404 };
                break;

            default:
                _logger.LogError(
                    context.Exception,
                    "unhandled exception in {Controller}.{Action}",
                    context.ActionDescriptor.RouteValues["controller"],
                    context.ActionDescriptor.RouteValues["action"]
                );
                context.Result = new ObjectResult("unhandled error occurred") { StatusCode = 500 };
                break;
        }

        context.ExceptionHandled = true;
    }
}
