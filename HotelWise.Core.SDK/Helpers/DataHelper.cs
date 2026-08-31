using System.Globalization;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de data/hora e cultura: formatação, conversão de segundos,
/// obtenção de horário do Brasil e aplicação de fuso horário.
/// Amplamente usado por logging, persistência e processamento de negócio.
/// </summary>
/// <example>
/// <code>
/// DataHelper.SetCulture();
/// var agoraBr = DataHelper.GetDateTimeNowBrazil();
/// var formatado = DataHelper.GetDateTimeCustomFormat(agoraBr);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class DataHelper
{
    public static string ConvertSecondsToTimeString(double seconds) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.ConvertSecondsToTimeString(seconds);

    public static string GetDateTimeCustomFormat(DateTime dateInput) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeCustomFormat(dateInput);

    public static void SetCulture() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.SetCulture();

    public static DateTime GetDateTimeNowBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowBrazil();

    public static DateTime GetDateTimeNowToLog() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToLog();

    public static DateTime GetDateTimeNowToProcess() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToProcess();

    public static DateTime GetDateTimeNowToPersistData() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToPersistData();

    public static DateTime GetDateTimeNow() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNow();

    public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.ApplyTimeZone(dateTime, timeZoneId);
}
