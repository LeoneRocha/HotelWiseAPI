#if NET8_0_OR_GREATER
using System.Diagnostics;
using HotelWise.Core.SDK.Common;
using Microsoft.Extensions.Configuration;
using Serilog;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Logging;

/// <summary>
/// Helpers de logging Serilog — casca sobre
/// <see cref="SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper"/>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper em SmartCoreHub.Core.SDK.")]
public static class LogAppHelper
{
    /// <summary>Calcula e formata o tempo decorrido do Stopwatch.</summary>
    /// <param name="stopwatch">Instância do Stopwatch.</param>
    /// <returns>String com o tempo formatado.</returns>
    public static string GetDurationStopwatch(Stopwatch stopwatch) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.GetDurationStopwatch(stopwatch);

    /// <summary>Registra uma exceção no logger Serilog com categoria.</summary>
    /// <param name="logger">Instância do logger Serilog.</param>
    /// <param name="ex">Exceção a ser registrada.</param>
    /// <param name="logType">Tipo ou contexto do log.</param>
    public static void LogException(ILogger logger, Exception ex, string logType) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.LogException(logger, ex, logType);

    /// <summary>Cria uma instância configurada de Serilog Logger.</summary>
    /// <param name="configuration">Configuração da aplicação.</param>
    /// <returns>Instância do Logger Serilog.</returns>
    public static Serilog.Core.Logger CreateLogger(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.CreateLogger(configuration);

    /// <summary>Obtém metadados de versão e ambiente do produto.</summary>
    /// <returns>DTO contendo as informações da aplicação.</returns>
    public static AppInformationVersionProductDto GetInformationVersionProduct()
    {
        var sch = SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.GetInformationVersionProduct();
        return new AppInformationVersionProductDto
        {
            Name = sch.Name ?? string.Empty,
            Version = sch.Version ?? string.Empty,
            EnvironmentName = sch.EnvironmentName ?? string.Empty,
            Message = sch.Message ?? string.Empty
        };
    }

    /// <summary>Retorna a string formatada com os dados de versão e ambiente.</summary>
    /// <returns>String descritiva do produto.</returns>
    public static string ShowInformationVersionProductString() =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.ShowInformationVersionProductString();

    /// <summary>Imprime no log as informações de versão do produto.</summary>
    /// <param name="logger">Instância do logger Serilog.</param>
    public static void PrintLogInformationVersionProduct(ILogger logger) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.PrintLogInformationVersionProduct(logger);

    /// <summary>Configura o valor da variável de ambiente ASPNETCORE_ENVIRONMENT.</summary>
    /// <param name="configuration">Configuração da aplicação.</param>
    public static void Set_ASPNETCORE_ENVIRONMENT(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(configuration);
}
#endif
