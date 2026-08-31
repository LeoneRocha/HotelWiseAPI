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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Middleware.Ported.RequestLoggingMiddleware. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class RequestLoggingMiddleware : SmartCoreHub.Core.SDK.Infrastructure.Middleware.Ported.RequestLoggingMiddleware
{
    public RequestLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        : base(next, logger)
    {
    }
}

#endif
