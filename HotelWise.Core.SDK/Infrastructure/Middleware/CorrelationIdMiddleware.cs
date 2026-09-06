#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Http;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure.Middleware;

/// <summary>
/// Middleware ASP.NET Core que propaga ou gera o cabeçalho <c>X-Correlation-ID</c>,
/// armazena o identificador em <see cref="HttpContext.Items"/> e enriquece o
/// LogContext do Serilog para rastreabilidade ponta a ponta.
/// </summary>
/// <remarks>
/// Registro no pipeline:
/// <code>
/// app.UseMiddleware&lt;CorrelationIdMiddleware&gt;();
/// </code>
/// </remarks>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.API.Middleware.CorrelationIdMiddleware", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.API.Middleware.CorrelationIdMiddleware em SmartCoreHub.Core.SDK.")]
public class CorrelationIdMiddleware : SmartCoreHub.Core.SDK.Service.API.Middleware.CorrelationIdMiddleware
{
    /// <summary>
    /// Alias legado HW do cabeçalho de correlação (<see cref="HeaderKey"/>).
    /// </summary>
    public const string HeaderName = HeaderKey;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CorrelationIdMiddleware"/>.
    /// </summary>
    /// <param name="next">Próximo delegate na esteira HTTP.</param>
    public CorrelationIdMiddleware(RequestDelegate next)
        : base(next)
    {
    }
}
#endif
