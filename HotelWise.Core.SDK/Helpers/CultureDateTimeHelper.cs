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
    public static List<SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto> GetTimeZonesIds() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetTimeZonesIds();

    public static List<SmartCoreHub.Core.SDK.Common.CultureDisplayDto> GetCultures() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetCultures();

    public static List<CultureInfo> TranslateCulture(List<SmartCoreHub.Core.SDK.Common.CultureDisplayDto> cultureDisplays) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.TranslateCulture(cultureDisplays);

    // Overload legado HW
    public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays) =>
        TranslateCulture(cultureDisplays.ConvertAll(c => (SmartCoreHub.Core.SDK.Common.CultureDisplayDto)c));

    public static string GetNameAndCulture(string localizedStringKeyName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetNameAndCulture(localizedStringKeyName);

    public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetKeyLocalizationRecordFormat(LanguageKey, Language);

    public static string GetTimeZoneBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetTimeZoneBrazil();

    public static string GetCultureBrazil() =>
        SmartCoreHub.Core.SDK.Domain.Helpers.Ported.CultureDateTimeHelper.GetCultureBrazil();
}
