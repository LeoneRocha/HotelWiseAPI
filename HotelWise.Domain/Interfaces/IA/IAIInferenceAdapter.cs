using HotelWise.Domain.Dto.IA.SemanticKernel;

namespace HotelWise.Domain.Interfaces.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAIInferenceAdapter.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAIInferenceAdapter
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages);
        Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages);
        Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages);
    }
}
