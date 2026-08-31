namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração Azure AD / Microsoft Entra ID.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAzureAdConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAzureAdConfig
{
}
