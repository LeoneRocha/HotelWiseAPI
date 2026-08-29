using HotelWise.Domain.Interfaces.AppConfig;

namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
/// </summary>
[Obsolete(
    "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Configuration.MistralApiEmbeddingsConfig.",
    error: false,
    DiagnosticId = "HW_CORE_SDK_AI")]
public class MistralApiEmbeddingsConfig : HotelWise.Core.SDK.AI.Configuration.MistralApiEmbeddingsConfig, IAiInferenceConfigBase
{
}
