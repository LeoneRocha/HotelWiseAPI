namespace HotelWise.Core.SDK.Common.Constants;

/// <summary>
/// Constantes de configuração relacionadas ao Azure AD / Microsoft Entra ID.
/// Utilizadas como chaves de seção de configuração e identificação do provedor de identidade.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.Constants.AzureADEntraIDConstants. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class AzureADEntraIDConstants
{
    /// <summary>
    /// Nome da seção de configuração do Azure AD / Entra ID (<c>AzureAD</c>).
    /// </summary>
    public const string AzureAd = "AzureAD";
}
