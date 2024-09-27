using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Dialogs.Api.Middleware;
public class ActiveRequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActiveRequestMiddleware> _logger;
    private static int _activeRequestCount = 0;

    public ActiveRequestMiddleware(RequestDelegate next, ILogger<ActiveRequestMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Interlocked.Increment(ref _activeRequestCount);
        try
        {
            _logger.LogInformation("Processing request: {Path}", context.Request.Path);
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            throw;
        }
        finally
        {
            Interlocked.Decrement(ref _activeRequestCount);
        }
    }

    public static int GetActiveRequestCount() => _activeRequestCount;
}
