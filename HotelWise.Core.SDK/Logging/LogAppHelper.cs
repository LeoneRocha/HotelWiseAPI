#if NET8_0_OR_GREATER
using System.Diagnostics;
using HotelWise.Core.SDK.Common;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HotelWise.Core.SDK.Logging;

/// <summary>
/// Helpers de logging Serilog — casca sobre
/// <see cref="SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper"/>.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class LogAppHelper
{
    public static string GetDurationStopwatch(Stopwatch stopwatch) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.GetDurationStopwatch(stopwatch);

    public static void LogException(ILogger logger, Exception ex, string logType) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.LogException(logger, ex, logType);

    public static Serilog.Core.Logger CreateLogger(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.CreateLogger(configuration);

    public static AppInformationVersionProductDto GetInformationVersionProduct()
    {
        var sch = SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.GetInformationVersionProduct();
        return new AppInformationVersionProductDto
        {
            Name = sch.Name,
            Version = sch.Version,
            EnvironmentName = sch.EnvironmentName,
            Message = sch.Message
        };
    }

    public static string ShowInformationVersionProductString() =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.ShowInformationVersionProductString();

    public static void PrintLogInformationVersionProduct(ILogger logger) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.PrintLogInformationVersionProduct(logger);

    public static void Set_ASPNETCORE_ENVIRONMENT(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(configuration);
}
#endif
