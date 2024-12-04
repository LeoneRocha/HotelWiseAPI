
using HotelWise.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HotelWise.Domain.CustomMiddleware
{
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
            var headers = context.Request.Headers.Where(x => x.Key.Contains("Authorization"));

            // Logando os cabeçalhos da requisição
            foreach (var header in headers)
            {
                _logger.Information($"{header.Key}: {header.Value}");
            }

            // Chamando o próximo middleware na pipeline
            await _next(context);
        }
    }
}
