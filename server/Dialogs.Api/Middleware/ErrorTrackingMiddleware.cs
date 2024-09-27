using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dialogs.Api.Middleware;
public class ErrorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorTrackingMiddleware> _logger;

    public ErrorTrackingMiddleware(RequestDelegate next, ILogger<ErrorTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occurred while processing your request.");
        }
    }
}
