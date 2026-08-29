using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAIInferenceService.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAIInferenceService
    {
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
        Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
        Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType);
        Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType);
    }
}
