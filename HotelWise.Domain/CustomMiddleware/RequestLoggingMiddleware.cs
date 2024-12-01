
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HotelWise.Domain.CustomMiddleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Request.Headers;

            // Logando os cabeçalhos da requisição
            foreach (var header in headers)
            {
                _logger.LogInformation($"{header.Key}: {header.Value}");
            }

            // Chamando o próximo middleware na pipeline
            await _next(context);
        }
    }
}
