using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato agregado de configuração de IA da aplicação.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig em SmartCoreHub.Core.SDK.")]
public interface IApplicationIAConfig
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig
{
}
