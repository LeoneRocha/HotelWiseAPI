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
    /// <summary>
    /// Converte uma quantidade de segundos em string no formato <c>hh:mm:ss</c>.
    /// </summary>
    /// <param name="seconds">Total de segundos.</param>
    /// <returns>Tempo formatado como <c>hh:mm:ss</c>.</returns>
    public static string ConvertSecondsToTimeString(double seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"hh\:mm\:ss");
    }

    /// <summary>
    /// Formato padrão de data/hora usado por <see cref="GetDateTimeCustomFormat"/>.
    /// </summary>
    private static readonly string dateFormat = "dd/MM/yyyy HH:mm:ss";

    /// <summary>
    /// Formata uma data no padrão <c>dd/MM/yyyy HH:mm:ss</c> com cultura invariante.
    /// </summary>
    /// <param name="dateInput">Data/hora a formatar.</param>
    /// <returns>String formatada.</returns>
    public static string GetDateTimeCustomFormat(DateTime dateInput)
    {
        var cultureInfo = CultureInfo.InvariantCulture;
        var result = dateInput.ToString(dateFormat, cultureInfo);
        return result;
    }

    /// <summary>
    /// Define a cultura do thread atual como pt-BR (CurrentCulture e CurrentUICulture).
    /// </summary>
    public static void SetCulture()
    {
        var cultureInfo = new CultureInfo("pt-BR");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    /// <summary>
    /// Obtém a data/hora atual convertida para o fuso <c>E. South America Standard Time</c> (Brasil).
    /// </summary>
    /// <returns>Data/hora no horário de Brasília.</returns>
    public static DateTime GetDateTimeNowBrazil()
    {
        DateTime now = DateTime.UtcNow;
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        DateTime brazilTime = TimeZoneInfo.ConvertTimeFromUtc(now, tzi);
        return brazilTime;
    }

    /// <summary>
    /// Alias de <see cref="GetDateTimeNowBrazil"/> para timestamps de log.
    /// </summary>
    /// <returns>Data/hora atual no fuso do Brasil.</returns>
    public static DateTime GetDateTimeNowToLog() => GetDateTimeNowBrazil();

    /// <summary>
    /// Alias de <see cref="GetDateTimeNowBrazil"/> para processamento de negócio.
    /// </summary>
    /// <returns>Data/hora atual no fuso do Brasil.</returns>
    public static DateTime GetDateTimeNowToProcess() => GetDateTimeNowBrazil();

    /// <summary>
    /// Alias de <see cref="GetDateTimeNowBrazil"/> para persistência de dados.
    /// </summary>
    /// <returns>Data/hora atual no fuso do Brasil.</returns>
    public static DateTime GetDateTimeNowToPersistData() => GetDateTimeNowBrazil();

    /// <summary>
    /// Obtém a data/hora atual em UTC.
    /// </summary>
    /// <returns><see cref="DateTime.UtcNow"/>.</returns>
    public static DateTime GetDateTimeNow() => DateTime.UtcNow;

    /// <summary>
    /// Converte uma data UTC para o fuso horário informado.
    /// </summary>
    /// <param name="dateTime">Data/hora em UTC.</param>
    /// <param name="timeZoneId">Identificador do fuso (ex.: <c>E. South America Standard Time</c>).</param>
    /// <returns>Data/hora convertida para o fuso solicitado.</returns>
    public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
        return dateTimeWithTimeZone;
    }
}
