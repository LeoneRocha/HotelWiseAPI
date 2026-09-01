using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração relacionadas ao Azure AD / Microsoft Entra ID.
/// Utilizadas como chaves de seção de configuração e identificação do provedor de identidade.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.Constants.AzureADEntraIDConstants", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.Constants.AzureADEntraIDConstants em SmartCoreHub.Core.SDK.")]
public static class AzureADEntraIDConstants
{
    /// <summary>Nome da seção de configuração do Azure AD / Microsoft Entra ID.</summary>
    public const string AzureAd =
        SmartCoreHub.Core.SDK.Common.Constants.AzureADEntraIDConstants.AzureAd;
}
