using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial embutido em prompts RAG.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO em SmartCoreHub.Core.SDK.")]
public class DataVectorVO : SchDto.DataVectorVO
{
}

/// <summary>
/// Mensagem de prompt para adapters de inferência — herda SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO em SmartCoreHub.Core.SDK.")]
public class PromptMessageVO : SchDto.PromptMessageVO
{
}
