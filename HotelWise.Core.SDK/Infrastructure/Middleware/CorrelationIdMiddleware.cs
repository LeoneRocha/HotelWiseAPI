#if NET8_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace HotelWise.Core.SDK.Infrastructure.Middleware;

/// <summary>
/// Middleware ASP.NET Core que propaga ou gera o cabeçalho <c>X-Correlation-ID</c>,
/// armazena o identificador em <see cref="HttpContext.Items"/> e enriquece o
/// <see cref="LogContext"/> do Serilog para rastreabilidade ponta a ponta.
/// </summary>
/// <remarks>
/// Registro no pipeline:
/// <code>
/// app.UseMiddleware&lt;CorrelationIdMiddleware&gt;();
/// </code>
/// </remarks>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Middleware.Ported.CorrelationIdMiddleware. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class CorrelationIdMiddleware : SmartCoreHub.Core.SDK.Infrastructure.Middleware.Ported.CorrelationIdMiddleware
{
    public CorrelationIdMiddleware(RequestDelegate next)
        : base(next)
    {
    }
}

#endif
