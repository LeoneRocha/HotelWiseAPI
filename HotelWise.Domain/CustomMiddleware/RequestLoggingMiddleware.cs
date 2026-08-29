using Microsoft.AspNetCore.Http;

namespace HotelWise.Domain.CustomMiddleware
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.Middleware.RequestLoggingMiddleware.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_MIDDLEWARE")]
    public class RequestLoggingMiddleware : HotelWise.Core.SDK.Infrastructure.Middleware.RequestLoggingMiddleware
    {
        public RequestLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
            : base(next, logger)
        {
        }
    }
}
