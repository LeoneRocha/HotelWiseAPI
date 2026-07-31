using HotelWise.Domain.AppException;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace HotelWise.Domain.CustomMiddleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var correlationId = context.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
                ?? context.TraceIdentifier;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
            using (Serilog.Context.LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (Serilog.Context.LogContext.PushProperty("RequestMethod", context.Request.Method))
            {
                LogAppHelper.LogException(_logger, ex, "API");
            }

            if (context.Response.HasStarted)
            {
                throw ex;
            }

            var isWarning = ex is AppWarningException;
            var statusCode = isWarning ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;

            var message = _environment.IsDevelopment() || isWarning
                ? ex.Message
                : "An unexpected error occurred.";

            var payload = new
            {
                Errors = new[]
                {
                    new ErrorResponse
                    {
                        Name = isWarning ? "AppWarning" : "UnhandledException",
                        Message = message,
                        ErrorCode = isWarning ? "APP_WARNING" : "UNHANDLED_EXCEPTION",
                        FullMessage = _environment.IsDevelopment() ? ex.ToString() : string.Empty
                    }
                },
                TraceId = correlationId,
                CorrelationId = correlationId
            };

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
