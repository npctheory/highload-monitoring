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
        Interlocked.Increment(ref _activeRequestCount); // Increment the active request count
        try
        {
            _logger.LogInformation("Processing request: {Path}", context.Request.Path);
            await _next(context); // Call the next middleware in the pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            throw; // Re-throw the exception to be handled by the next middleware
        }
        finally
        {
            Interlocked.Decrement(ref _activeRequestCount); // Decrement the active request count
        }
    }

    public static int GetActiveRequestCount() => _activeRequestCount; // Method to get the active request count
}
