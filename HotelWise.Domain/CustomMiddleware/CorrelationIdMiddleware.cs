using Microsoft.AspNetCore.Http;

namespace HotelWise.Domain.CustomMiddleware
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Infrastructure.Middleware.CorrelationIdMiddleware.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_MIDDLEWARE")]
    public class CorrelationIdMiddleware : HotelWise.Core.SDK.Infrastructure.Middleware.CorrelationIdMiddleware
    {
        public new const string HeaderName = HotelWise.Core.SDK.Infrastructure.Middleware.CorrelationIdMiddleware.HeaderName;
        public new const string ItemKey = HotelWise.Core.SDK.Infrastructure.Middleware.CorrelationIdMiddleware.ItemKey;

        public CorrelationIdMiddleware(RequestDelegate next) : base(next)
        {
        }
    }
}
