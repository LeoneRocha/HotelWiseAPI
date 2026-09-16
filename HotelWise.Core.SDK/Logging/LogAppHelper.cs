#if NET8_0_OR_GREATER
using System.Diagnostics;
using HotelWise.Core.SDK.Common;
using Microsoft.Extensions.Configuration;
using Serilog;
using SmartCoreHub.Core.SDK.Common.Attributes;
using SmartCoreHub.Core.SDK.Infrastructure.Logging;

namespace HotelWise.Core.SDK.Logging;

/// <summary>
/// Helpers de logging Serilog — casca sobre
/// <see cref="SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper"/>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper em SmartCoreHub.Core.SDK.")]
public static class LogAppHelper
{
    /// <summary>Calcula e formata o tempo decorrido do Stopwatch.</summary>
    public static string GetDurationStopwatch(Stopwatch stopwatch) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.GetDurationStopwatch(stopwatch);

    /// <summary>Registra uma exceção no logger Serilog com categoria.</summary>
    public static void LogException(ILogger logger, Exception ex, string logType) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.LogException(new SerilogAdapter(logger), ex, logType);

    /// <summary>Cria uma instância configurada de Serilog Logger.</summary>
    public static Serilog.Core.Logger CreateLogger(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.CreateLogger(configuration);

    /// <summary>Obtém metadados de versão e ambiente do produto.</summary>
    public static AppInformationVersionProductDto GetInformationVersionProduct()
    {
        var sch = SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.GetInformationVersionProduct();
        return new AppInformationVersionProductDto
        {
            Name = sch.Name ?? string.Empty,
            Version = sch.Version ?? string.Empty,
            EnvironmentName = sch.EnvironmentName ?? string.Empty,
            Message = sch.Message ?? string.Empty
        };
    }

    /// <summary>Retorna a string formatada com os dados de versão e ambiente.</summary>
    public static string ShowInformationVersionProductString() =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.ShowInformationVersionProductString();

    /// <summary>Imprime no log as informações de versão do produto.</summary>
    public static void PrintLogInformationVersionProduct(ILogger logger) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.PrintLogInformationVersionProduct(new SerilogAdapter(logger));

    /// <summary>Configura o valor da variável de ambiente ASPNETCORE_ENVIRONMENT.</summary>
    public static void Set_ASPNETCORE_ENVIRONMENT(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(configuration);
}
#endif
