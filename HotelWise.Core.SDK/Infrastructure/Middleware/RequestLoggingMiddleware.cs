#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure.Middleware;

/// <summary>
/// Middleware ASP.NET Core que registra de forma leve cada requisição HTTP
/// (método, caminho, status e duração), correlacionando o log ao correlation id
/// sem expor secrets ou payloads sensíveis.
/// </summary>
/// <remarks>
/// Registro no pipeline:
/// <code>
/// app.UseMiddleware&lt;CorrelationIdMiddleware&gt;();
/// app.UseMiddleware&lt;RequestLoggingMiddleware&gt;();
/// </code>
/// </remarks>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.API.Middleware.RequestLoggingMiddleware", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.API.Middleware.RequestLoggingMiddleware em SmartCoreHub.Core.SDK.")]
public class RequestLoggingMiddleware : SmartCoreHub.Core.SDK.Service.API.Middleware.RequestLoggingMiddleware
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RequestLoggingMiddleware"/>.
    /// </summary>
    /// <param name="next">Próximo delegate na esteira HTTP.</param>
    /// <param name="logger">Logger MS.Extensions para o middleware canônico SCH.</param>
    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<SmartCoreHub.Core.SDK.Service.API.Middleware.RequestLoggingMiddleware> logger)
        : base(next, logger)
    {
    }
}
#endif
