namespace HotelWise.Domain.Constants
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.Constants.AppConfigConstants.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public static class AppConfigConstants
    {
        public const string ApplicationContentJon = HotelWise.Core.SDK.Common.Constants.AppConfigConstants.ApplicationContentJon;
        public const string DATE_FORMAT = HotelWise.Core.SDK.Common.Constants.AppConfigConstants.DATE_FORMAT;
        public const string DATE_FORMAT2 = HotelWise.Core.SDK.Common.Constants.AppConfigConstants.DATE_FORMAT2;
        public const string ConfigurationConfigurationNotBeNull = HotelWise.Core.SDK.Common.Constants.AppConfigConstants.ConfigurationConfigurationNotBeNull;
    }
}
