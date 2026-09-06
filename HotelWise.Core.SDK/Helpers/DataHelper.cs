using System.Globalization;

using SmartCoreHub.Core.SDK.Common.Attributes;

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
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper em SmartCoreHub.Core.SDK.")]
public static class DataHelper
{
    /// <summary>Converte valor numérico em segundos para texto formatado HH:mm:ss.</summary>
    /// <param name="seconds">Tempo em segundos.</param>
    /// <returns>Tempo formatado.</returns>
    public static string ConvertSecondsToTimeString(double seconds) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.ConvertSecondsToTimeString(seconds);

    /// <summary>Formata uma data para o formato padrão do sistema.</summary>
    /// <param name="dateInput">Data de entrada.</param>
    /// <returns>Data formatada em string.</returns>
    public static string GetDateTimeCustomFormat(DateTime dateInput) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeCustomFormat(dateInput);

    /// <summary>Configura a cultura global do thread atual para o padrão pt-BR.</summary>
    public static void SetCulture() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.SetCulture();

    /// <summary>Obtém a data e hora atual no fuso horário do Brasil.</summary>
    /// <returns>Data/hora atual de Brasília.</returns>
    public static DateTime GetDateTimeNowBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowBrazil();

    /// <summary>Obtém a data e hora atual destinada a registros de log.</summary>
    /// <returns>Data/hora para log.</returns>
    public static DateTime GetDateTimeNowToLog() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToLog();

    /// <summary>Obtém a data e hora atual destinada a processamento de regras.</summary>
    /// <returns>Data/hora de processamento.</returns>
    public static DateTime GetDateTimeNowToProcess() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToProcess();

    /// <summary>Obtém a data e hora atual destinada à persistência de dados.</summary>
    /// <returns>Data/hora de persistência.</returns>
    public static DateTime GetDateTimeNowToPersistData() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNowToPersistData();

    /// <summary>Obtém a data e hora atual padrão do sistema.</summary>
    /// <returns>Data/hora atual.</returns>
    public static DateTime GetDateTimeNow() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.GetDateTimeNow();

    /// <summary>Aplica o fuso horário especificado a uma data/hora.</summary>
    /// <param name="dateTime">Data e hora original.</param>
    /// <param name="timeZoneId">Identificador do fuso horário de destino.</param>
    /// <returns>Data/hora convertida para o fuso.</returns>
    public static DateTime ApplyTimeZone(DateTime dateTime, string timeZoneId) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.DataHelper.ApplyTimeZone(dateTime, timeZoneId);
}
