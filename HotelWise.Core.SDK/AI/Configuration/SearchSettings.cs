using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configurações auxiliares de busca vetorial / RAG — herda SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings em SmartCoreHub.Core.SDK.")]
public class SearchSettings : SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings
{
}
