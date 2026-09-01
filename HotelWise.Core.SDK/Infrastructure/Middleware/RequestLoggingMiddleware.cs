#if NET8_0_OR_GREATER
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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
public class RequestLoggingMiddleware : SmartCoreHub.Core.SDK.Infrastructure.Middleware.Ported.RequestLoggingMiddleware
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="RequestLoggingMiddleware"/>.
    /// </summary>
    /// <param name="next">Próximo delegate na esteira HTTP.</param>
    /// <param name="logger">Instância do logger Serilog.</param>
    public RequestLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        : base(next, logger)
    {
    }
}
#endif
