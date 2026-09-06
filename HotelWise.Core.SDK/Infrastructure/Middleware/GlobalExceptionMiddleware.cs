#if NET8_0_OR_GREATER
using System.Text.Json;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure.Middleware;

/// <summary>
/// Middleware ASP.NET Core que captura exceções não tratadas no pipeline,
/// registra o erro com correlation id e devolve um payload JSON padronizado
/// (<see cref="ErrorResponse"/>), diferenciando avisos de aplicação de falhas internas.
/// </summary>
/// <remarks>
/// Registro no pipeline (tipicamente no início):
/// <code>
/// app.UseMiddleware&lt;GlobalExceptionMiddleware&gt;();
/// </code>
/// </remarks>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware em SmartCoreHub.Core.SDK.")]
public class GlobalExceptionMiddleware : SmartCoreHub.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GlobalExceptionMiddleware"/>.
    /// </summary>
    /// <param name="next">Próximo delegate na esteira HTTP.</param>
    /// <param name="logger">Instância do logger Serilog.</param>
    /// <param name="environment">Ambiente de hospedagem da aplicação.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IWebHostEnvironment environment)
        : base(next, logger, environment)
    {
    }
}
#endif
