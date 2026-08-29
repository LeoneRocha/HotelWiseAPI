namespace HotelWise.Domain.Constants
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.Constants.AzureADEntraIDConstants.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public static class AzureADEntraIDConstants
    {
        public const string AzureAd = HotelWise.Core.SDK.Common.Constants.AzureADEntraIDConstants.AzureAd;
    }
}
