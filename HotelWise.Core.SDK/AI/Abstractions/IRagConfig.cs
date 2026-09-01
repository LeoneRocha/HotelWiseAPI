using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração RAG (Retrieval-Augmented Generation).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig em SmartCoreHub.Core.SDK.")]
public interface IRagConfig
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig
{
}
