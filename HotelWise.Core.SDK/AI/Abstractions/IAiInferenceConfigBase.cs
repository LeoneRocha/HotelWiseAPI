using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato base de configuração de inferência IA.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase em SmartCoreHub.Core.SDK.")]
public interface IAiInferenceConfigBase : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase
{
}
