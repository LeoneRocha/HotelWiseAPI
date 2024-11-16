using HotelWise.Domain.Dto;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace HotelWise.Domain.Helpers
{
    public static class CultureDateTimeHelper
    {   
        private static List<CultureInfo> getCulturesEnable()
        {
            List<CultureInfo> list = new List<CultureInfo>();

            list.Add(new CultureInfo("en-US"));
            list.Add(new CultureInfo("pt-BR"));
            list.Add(new CultureInfo("es-ES"));

            return list;
        }

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
        public static List<CultureDisplayDto> GetCultures()
        {
            List<CultureDisplayDto> result = new List<CultureDisplayDto>();
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

            foreach (CultureInfo cul in cinfo)
            {
                result.Add(new CultureDisplayDto() { Id = cul.Name, Name = cul.DisplayName });
            }
            var culturesEnables = getCulturesEnable().Select(cie => cie.Name).ToList();
            result = result.Where(ci => culturesEnables.Contains(ci.Id)).ToList();

            return result;
        }

        public static List<CultureInfo> TranslateCulture(List<CultureDisplayDto> cultureDisplays)
        {
            return cultureDisplays.Select(cd => new CultureInfo(cd.Id)).ToList();
        }

        public static string GetNameAndCulture(string localizedStringKeyName)
        {  
            return $"{localizedStringKeyName}"; 
        }
        public static string GetKeyLocalizationRecordFormat(string LanguageKey, string Language)
        {
            return $"{LanguageKey}";
        }
         
        public static string GetTimeZoneBrazil()
        {
            var zt = CultureDateTimeHelper.GetTimeZonesIds().Find(c =>
             c.Name.Contains("o Paulo", StringComparison.OrdinalIgnoreCase)
             || c.Id.Contains("o Paulo", StringComparison.OrdinalIgnoreCase)
             || c.Name.Contains("Brasília", StringComparison.OrdinalIgnoreCase)
             || c.Id.Contains("Brasília", StringComparison.OrdinalIgnoreCase)
             || c.Id.Contains("South America", StringComparison.OrdinalIgnoreCase)
             );
            string idZT = "E. South America Standard Time";
            if (zt != null)
            {
                idZT = zt.Id;
            }
            return idZT;
        }

        public static string GetCultureBrazil()
        {
            return CultureDateTimeHelper.GetCultures().First(c => c.Id.Contains("pt-br", StringComparison.OrdinalIgnoreCase)).Id;
        }
    }
}
