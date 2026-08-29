using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HotelWise.Domain.CustomMiddleware
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_MIDDLEWARE")]
    public class GlobalExceptionMiddleware : HotelWise.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware
    {
        public GlobalExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IWebHostEnvironment environment)
            : base(next, logger, environment)
        {
        }
    }
}
