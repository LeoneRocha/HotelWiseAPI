using System.Collections.ObjectModel;
using System.Globalization;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de cultura e fuso horário para listagem, conversão e resolução
/// de culturas habilitadas (en-US, pt-BR, es-ES) e timezone do Brasil.
/// Usado por telas de configuração e fluxos de localização do SDK.
/// </summary>
/// <example>
/// <code>
/// var cultures = CultureDateTimeHelper.GetCultures();
/// var brazilTz = CultureDateTimeHelper.GetTimeZoneBrazil();
/// </code>
/// </example>
public static class CultureDateTimeHelper
{
    /// <summary>
    /// Retorna a lista fixa de culturas habilitadas pelo produto.
    /// </summary>
    /// <returns>Culturas en-US, pt-BR e es-ES.</returns>
    private static List<CultureInfo> GetCulturesEnable()
    {
        List<CultureInfo> list = new List<CultureInfo>();
        list.Add(new CultureInfo("en-US"));
        list.Add(new CultureInfo("pt-BR"));
        list.Add(new CultureInfo("es-ES"));
        return list;
    }

    /// <summary>
    /// Lista todos os fusos horários do sistema operacional como DTOs de exibição.
    /// </summary>
    /// <returns>Lista de <see cref="TimeZoneDisplayDto"/> com Id e nome amigável.</returns>
    public static List<TimeZoneDisplayDto> GetTimeZonesIds()
    {
        List<TimeZoneDisplayDto> result = new List<TimeZoneDisplayDto>();
        ReadOnlyCollection<TimeZoneInfo> tz = TimeZoneInfo.GetSystemTimeZones();
        foreach (TimeZoneInfo tzInfo in tz)
        {
            result.Add(new TimeZoneDisplayDto() { Id = tzInfo.Id, Name = tzInfo.DisplayName });
        }
        return result;
    }

    /// <summary>
    /// Lista as culturas específicas habilitadas pelo produto (filtradas de AllCultures).
    /// </summary>
    /// <returns>Lista de <see cref="CultureDisplayDto"/> permitidos.</returns>
    public static List<CultureDisplayDto> GetCultures()
    {
        List<CultureDisplayDto> result = new List<CultureDisplayDto>();
        CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

        foreach (CultureInfo cul in cinfo)
        {
            result.Add(new CultureDisplayDto() { Id = cul.Name, Name = cul.DisplayName });
        }

        var culturesEnables = GetCulturesEnable().Select(cie => cie.Name).ToList();
        result = result.Where(ci => culturesEnables.Contains(ci.Id)).ToList();
        return result;
    }

    /// <summary>
    /// Converte DTOs de cultura em instâncias de <see cref="CultureInfo"/>.
    /// </summary>
    /// <param name="cultureDisplays">Lista de culturas em formato de exibição.</param>
    /// <returns>Lista de <see cref="CultureInfo"/> correspondente.</returns>
    public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays)
    {
        return cultureDisplays.Select(cd => new CultureInfo(cd.Id)).ToList();
    }

    /// <summary>
    /// Retorna a chave de localização no formato esperado (pass-through da chave).
    /// </summary>
    /// <param name="localizedStringKeyName">Nome da chave de recurso localizado.</param>
    /// <returns>A própria chave informada.</returns>
    public static string GetNameAndCulture(string localizedStringKeyName) => $"{localizedStringKeyName}";

    /// <summary>
    /// Monta o formato de chave de registro de localização (pass-through da LanguageKey).
    /// </summary>
    /// <param name="LanguageKey">Chave do idioma/recurso.</param>
    /// <param name="Language">Código do idioma (não utilizado no formato atual).</param>
    /// <returns>A LanguageKey informada.</returns>
    public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language) => $"{LanguageKey}";

    /// <summary>
    /// Resolve o Id do fuso horário do Brasil (São Paulo / Brasília / South America),
    /// com fallback para <c>E. South America Standard Time</c>.
    /// </summary>
    /// <returns>Identificador do timezone do Brasil no SO.</returns>
    public static string GetTimeZoneBrazil()
    {
        var zt = GetTimeZonesIds().Find(c =>
            c.Name.Contains("o Paulo", StringComparison.OrdinalIgnoreCase)
            || c.Id.Contains("o Paulo", StringComparison.OrdinalIgnoreCase)
            || c.Name.Contains("Brasília", StringComparison.OrdinalIgnoreCase)
            || c.Id.Contains("Brasília", StringComparison.OrdinalIgnoreCase)
            || c.Id.Contains("South America", StringComparison.OrdinalIgnoreCase));

        string idZT = "E. South America Standard Time";
        if (zt != null)
        {
            idZT = zt.Id;
        }
        return idZT;
    }

    /// <summary>
    /// Obtém o identificador da cultura brasileira (pt-BR) entre as habilitadas.
    /// </summary>
    /// <returns>Id da cultura pt-BR.</returns>
    public static string GetCultureBrazil()
    {
        return GetCultures().First(c => c.Id.Contains("pt-br", StringComparison.OrdinalIgnoreCase)).Id;
    }
}
