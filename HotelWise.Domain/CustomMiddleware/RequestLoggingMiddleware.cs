using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HotelWise.Domain.CustomMiddleware
{
    /// <summary>
    /// Lightweight request log without secrets. Prefer SerilogRequestLogging for HTTP access metrics;
    /// this middleware keeps a simple start/end breadcrumb with correlation.
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
}
