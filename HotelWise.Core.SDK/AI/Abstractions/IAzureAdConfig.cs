using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração Azure AD / Microsoft Entra ID.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAzureAdConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAzureAdConfig em SmartCoreHub.Core.SDK.")]
public interface IAzureAdConfig : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAzureAdConfig
{
}
