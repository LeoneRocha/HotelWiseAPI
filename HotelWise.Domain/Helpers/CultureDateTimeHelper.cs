using HotelWise.Domain.Dto;
using System.Globalization;

namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_HELPER")]
    public static class CultureDateTimeHelper
    {
        public static List<TimeZoneDisplayDto> GetTimeZonesIds() =>
            HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetTimeZonesIds()
                .Select(x => new TimeZoneDisplayDto { Id = x.Id, Name = x.Name }).ToList();

        public static List<CultureDisplayDto> GetCultures() =>
            HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetCultures()
                .Select(x => new CultureDisplayDto { Id = x.Id, Name = x.Name }).ToList();

        public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays) =>
            HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.TranslateCulture(
                cultureDisplays.Select(cd => new HotelWise.Core.SDK.Common.CultureDisplayDto { Id = cd.Id, Name = cd.Name }).ToList());

        public static string GetNameAndCulture(string localizedStringKeyName) =>
            HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetNameAndCulture(localizedStringKeyName);

        public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language) =>
            HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetKeyLocalizationRecordFormat(LanguageKey, Language);

        public static string GetTimeZoneBrazil() => HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetTimeZoneBrazil();

        public static string GetCultureBrazil() => HotelWise.Core.SDK.Helpers.CultureDateTimeHelper.GetCultureBrazil();
    }
}
