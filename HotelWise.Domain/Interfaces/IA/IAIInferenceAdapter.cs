using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Interfaces.IA
{
    public interface IAIInferenceAdapter
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages);
        Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages);
        Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages);
    }
}