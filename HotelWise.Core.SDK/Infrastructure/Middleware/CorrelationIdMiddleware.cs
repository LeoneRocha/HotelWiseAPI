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
public class CorrelationIdMiddleware
{
    /// <summary>
    /// Nome do cabeçalho HTTP usado para correlation id.
    /// </summary>
    public const string HeaderName = "X-Correlation-ID";

    /// <summary>
    /// Chave em <see cref="HttpContext.Items"/> e propriedade do LogContext Serilog.
    /// </summary>
    public const string ItemKey = "CorrelationId";

    /// <summary>
    /// Delegate do próximo middleware no pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa o middleware de correlation id.
    /// </summary>
    /// <param name="next">Próximo middleware do pipeline.</param>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Garante um correlation id na requisição, ecoa no response e propaga no LogContext.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    /// <returns>Tarefa que representa a execução assíncrona do pipeline.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.TraceIdentifier = correlationId;
        context.Items[ItemKey] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty(ItemKey, correlationId))
        {
            await _next(context);
        }
    }
}
#endif
