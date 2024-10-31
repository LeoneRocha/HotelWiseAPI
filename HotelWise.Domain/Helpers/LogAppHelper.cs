using HotelWise.Domain.AppException;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Diagnostics;

namespace HotelWise.Domain.Helpers
{
    public static class LogAppHelper
    {
        public static string GetDurationStopwatch(Stopwatch stopwatch)
        {
            return TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss");
        }
        public static void LogException(Serilog.ILogger logger, Exception ex, string logType)
        {
            var message = $"{logType}-LEVEL: {ex.Message} at: {DateTime.UtcNow}";
            if (ex is AppWarningException)
            {
                message = message.Replace("-LEVEL:", "-Warning:");
                logger.Warning(message);
            }
            else
            {
                message = message.Replace("-LEVEL:", "-Error:");
                logger.Error(ex, message);
            }
        }
        public static Serilog.Core.Logger CreateLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                      .ReadFrom.Configuration(configuration)
                      .Enrich.FromLogContext()
                      .CreateLogger();
        } 
    }
}
