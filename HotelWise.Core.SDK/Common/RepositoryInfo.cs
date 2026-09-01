using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Par interface/implementação usado no registro de repositórios em injeção de dependência.
/// Associa o tipo do contrato ao tipo concreto a ser resolvido pelo container.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.RepositoryInfo", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.RepositoryInfo em SmartCoreHub.Core.SDK.")]
public class RepositoryInfo : SmartCoreHub.Core.SDK.Common.RepositoryInfo
{

}
