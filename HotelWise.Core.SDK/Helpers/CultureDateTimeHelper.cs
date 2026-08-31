using System.Collections.ObjectModel;
using System.Globalization;
using HotelWise.Core.SDK.Common;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Utilitários de cultura e fuso horário.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class CultureDateTimeHelper
{
    /// <summary>Obtém a lista de identificadores de fusos horários disponíveis.</summary>
    /// <returns>Lista de fusos horários.</returns>
    public static List<SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto> GetTimeZonesIds() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetTimeZonesIds();

    /// <summary>Obtém a lista de culturas suportadas no sistema.</summary>
    /// <returns>Lista de culturas disponíveis.</returns>
    public static List<SmartCoreHub.Core.SDK.Common.CultureDisplayDto> GetCultures() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetCultures();

    /// <summary>Traduz e converte lista de culturas SCH para <see cref="CultureInfo"/>.</summary>
    /// <param name="cultureDisplays">Lista de culturas para tradução.</param>
    /// <returns>Lista de CultureInfo traduzidos.</returns>
    public static List<CultureInfo> TranslateCulture(List<SmartCoreHub.Core.SDK.Common.CultureDisplayDto> cultureDisplays) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.TranslateCulture(cultureDisplays);

    /// <summary>Traduz e converte lista de culturas HW para <see cref="CultureInfo"/>.</summary>
    /// <param name="cultureDisplays">Lista de culturas legado HW.</param>
    /// <returns>Lista de CultureInfo traduzidos.</returns>
    public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays) =>
        TranslateCulture(cultureDisplays.ConvertAll(c => (SmartCoreHub.Core.SDK.Common.CultureDisplayDto)c));

    /// <summary>Obtém o nome e cultura da string de chave localizada.</summary>
    /// <param name="localizedStringKeyName">Nome da chave localizada.</param>
    /// <returns>Nome e cultura.</returns>
    public static string GetNameAndCulture(string localizedStringKeyName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetNameAndCulture(localizedStringKeyName);

    /// <summary>Obtém o formato padrão de chave de localização.</summary>
    /// <param name="LanguageKey">Chave do idioma.</param>
    /// <param name="Language">Nome do idioma.</param>
    /// <returns>Chave formatada de registro de localização.</returns>
    public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetKeyLocalizationRecordFormat(LanguageKey, Language);

    /// <summary>Obtém o identificador de fuso horário do Brasil.</summary>
    /// <returns>Identificador do fuso horário brasileiro.</returns>
    public static string GetTimeZoneBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetTimeZoneBrazil();

    /// <summary>Obtém a cultura padrão do Brasil (pt-BR).</summary>
    /// <returns>Cultura brasileira.</returns>
    public static string GetCultureBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetCultureBrazil();
}
