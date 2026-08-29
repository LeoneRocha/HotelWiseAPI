#if NET8_0_OR_GREATER
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace HotelWise.Core.SDK.Infrastructure.Middleware;

/// <summary>
/// Log leve de request HTTP com correlation id (sem secrets).
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public RequestLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
            ?? context.TraceIdentifier;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.Information(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms (CorrelationId={CorrelationId})",
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
    }
}
#endif
