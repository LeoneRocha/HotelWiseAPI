using HotelWise.Domain.Dto;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Logging.LogAppHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_LOGGING")]
    public static class LogAppHelper
    {
        public static string GetDurationStopwatch(Stopwatch stopwatch) =>
            HotelWise.Core.SDK.Logging.LogAppHelper.GetDurationStopwatch(stopwatch);

        public static void LogException(ILogger logger, Exception ex, string logType) =>
            HotelWise.Core.SDK.Logging.LogAppHelper.LogException(logger, ex, logType);

        public static Serilog.Core.Logger CreateLogger(IConfiguration configuration) =>
            HotelWise.Core.SDK.Logging.LogAppHelper.CreateLogger(configuration);

        public static AppInformationVersionProductDto GetInformationVersionProduct()
        {
            var core = HotelWise.Core.SDK.Logging.LogAppHelper.GetInformationVersionProduct();
            return new AppInformationVersionProductDto
            {
                Id = core.Id,
                Name = core.Name,
                Version = core.Version,
                EnvironmentName = core.EnvironmentName,
                Message = core.Message
            };
        }

        public static string ShowInformationVersionProductString() =>
            HotelWise.Core.SDK.Logging.LogAppHelper.ShowInformationVersionProductString();

        public static void PrintLogInformationVersionProduct(ILogger logger) =>
            HotelWise.Core.SDK.Logging.LogAppHelper.PrintLogInformationVersionProduct(logger);

        public static void Set_ASPNETCORE_ENVIRONMENT(IConfiguration configuration) =>
            HotelWise.Core.SDK.Logging.LogAppHelper.Set_ASPNETCORE_ENVIRONMENT(configuration);
    }
}
