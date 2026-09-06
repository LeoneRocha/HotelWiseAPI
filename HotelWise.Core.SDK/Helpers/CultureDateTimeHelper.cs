using System.Globalization;
using HotelWise.Core.SDK.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de cultura e fuso horário.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper em SmartCoreHub.Core.SDK.")]
public static class CultureDateTimeHelper
{
    /// <summary>Obtém a lista de identificadores de fusos horários disponíveis.</summary>
    /// <returns>Lista de fusos horários.</returns>
    public static List<SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto> GetTimeZonesIds() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetTimeZonesIds();

    /// <summary>Obtém a lista de culturas suportadas no sistema.</summary>
    /// <returns>Lista de culturas disponíveis.</returns>
    public static List<SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto> GetCultures() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetCultures();

    /// <summary>Traduz e converte lista de culturas SCH para <see cref="CultureInfo"/>.</summary>
    /// <param name="cultureDisplays">Lista de culturas para tradução.</param>
    /// <returns>Lista de CultureInfo traduzidos.</returns>
    public static List<CultureInfo> TranslateCulture(List<SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto> cultureDisplays) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.TranslateCulture(cultureDisplays);

    /// <summary>Traduz e converte lista de culturas HW para <see cref="CultureInfo"/>.</summary>
    /// <param name="cultureDisplays">Lista de culturas legado HW.</param>
    /// <returns>Lista de CultureInfo traduzidos.</returns>
    public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays) =>
        TranslateCulture(cultureDisplays.ConvertAll(c => (SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto)c));

    /// <summary>Obtém o nome e cultura da string de chave localizada.</summary>
    /// <param name="localizedStringKeyName">Nome da chave localizada.</param>
    /// <returns>Nome e cultura.</returns>
    public static string GetNameAndCulture(string localizedStringKeyName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetNameAndCulture(localizedStringKeyName);

    /// <summary>Obtém o formato padrão de chave de localização.</summary>
    /// <param name="LanguageKey">Chave do idioma.</param>
    /// <param name="Language">Nome do idioma.</param>
    /// <returns>Chave formatada de registro de localização.</returns>
    public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetKeyLocalizationRecordFormat(LanguageKey, Language);

    /// <summary>Obtém o identificador de fuso horário do Brasil.</summary>
    /// <returns>Identificador do fuso horário brasileiro.</returns>
    public static string GetTimeZoneBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetTimeZoneBrazil();

    /// <summary>Obtém a cultura padrão do Brasil (pt-BR).</summary>
    /// <returns>Cultura brasileira.</returns>
    public static string GetCultureBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.CultureDateTimeHelper.GetCultureBrazil();
}
