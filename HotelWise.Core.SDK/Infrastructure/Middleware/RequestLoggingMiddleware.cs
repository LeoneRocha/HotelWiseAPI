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
public class RequestLoggingMiddleware
{
    /// <summary>
    /// Delegate do próximo middleware no pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger Serilog usado para emitir o log de request.
    /// </summary>
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Inicializa o middleware de logging de requisições.
    /// </summary>
    /// <param name="next">Próximo middleware do pipeline.</param>
    /// <param name="logger">Instância Serilog para gravação dos logs.</param>
    public RequestLoggingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Mede a duração da requisição e registra método, path, status e correlation id.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    /// <returns>Tarefa que representa a execução assíncrona do pipeline.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
            ?? context.TraceIdentifier;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.Information(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs} ms (CorrelationId={CorrelationId})",
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
    }
}
#endif
