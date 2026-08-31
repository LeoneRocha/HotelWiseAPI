#if NET8_0_OR_GREATER
using System.Text.Json;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Middleware.GlobalExceptionMiddleware. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GlobalExceptionMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Delegate do próximo middleware no pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger Serilog para gravação de exceções.
    /// </summary>
    private readonly Serilog.ILogger _logger;

    /// <summary>
    /// Ambiente de hospedagem (Development/Production) para decidir o detalhe da mensagem.
    /// </summary>
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Inicializa o middleware global de tratamento de exceções.
    /// </summary>
    /// <param name="next">Próximo middleware do pipeline.</param>
    /// <param name="logger">Instância Serilog para logging.</param>
    /// <param name="environment">Ambiente de hospedagem web.</param>
    public GlobalExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Executa o pipeline e, em caso de exceção, delega ao manipulador padronizado.
    /// </summary>
    /// <param name="context">Contexto HTTP da requisição atual.</param>
    /// <returns>Tarefa que representa a execução assíncrona.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Registra a exceção e escreve a resposta JSON de erro no <see cref="HttpResponse"/>.
    /// </summary>
    /// <param name="context">Contexto HTTP.</param>
    /// <param name="ex">Exceção capturada.</param>
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var correlationId = context.Items[CorrelationIdMiddleware.ItemKey]?.ToString()
            ?? context.TraceIdentifier;

        using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
        using (Serilog.Context.LogContext.PushProperty("RequestPath", context.Request.Path.Value))
        using (Serilog.Context.LogContext.PushProperty("RequestMethod", context.Request.Method))
        {
            LogAppHelper.LogException(_logger, ex, "API");
        }

        if (context.Response.HasStarted)
        {
            throw ex;
        }

        var isWarning = ex is AppWarningException;
        var statusCode = isWarning ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;

        var message = _environment.IsDevelopment() || isWarning
            ? ex.Message
            : "An unexpected error occurred.";

        var payload = new
        {
            Errors = new[]
            {
                new ErrorResponse
                {
                    Name = isWarning ? "AppWarning" : "UnhandledException",
                    Message = message,
                    ErrorCode = isWarning ? "APP_WARNING" : "UNHANDLED_EXCEPTION",
                    FullMessage = _environment.IsDevelopment() ? ex.ToString() : string.Empty
                }
            },
            TraceId = correlationId,
            CorrelationId = correlationId
        };

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(payload, JsonOptions);

        await context.Response.WriteAsync(json, context.RequestAborted);
    }
}
#endif
