using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;

namespace HotelWise.Service.Logging;

/// <summary>
/// Compat Serilog-style (.Error) sobre <see cref="IAppLogger"/> enquanto o HotelWise migra para LogError.
/// </summary>
public static class AppLoggerSerilogCompatExtensions
{
    public static void Error(this IAppLogger logger, Exception ex, string message, params object?[] args)
        => logger.LogError(ex, message, args);

    public static void Error(this IAppLogger logger, string message, params object?[] args)
        => logger.LogError(message, args);

    public static void Warning(this IAppLogger logger, Exception ex, string message, params object?[] args)
        => logger.LogWarning(ex, message, args);

    public static void Warning(this IAppLogger logger, string message, params object?[] args)
        => logger.LogWarning(message, args);

    public static void Information(this IAppLogger logger, string message, params object?[] args)
        => logger.LogInfo(message, args);
}
