using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com dados de segurança usados na geração e contextualização de tokens.
/// Transporta identidade, papel e chave de configuração associada ao usuário autenticado.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.SecurityDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.SecurityDto em SmartCoreHub.Core.SDK.")]
public class SecurityDto : SmartCoreHub.Core.SDK.Common.SecurityDto
{

}
