using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Service.AI
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete (PromptMessageVO/enums Domain ≠ Core).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Services.AIInferenceService.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class AIInferenceService : IAIInferenceService
    {
        private readonly IAIInferenceAdapterFactory _adapterFactory;

        public AIInferenceService(IConfiguration configuration, IAIInferenceAdapterFactory adapterFactory)
        {
            _adapterFactory = adapterFactory;
        }

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
        {
            var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await adapter!.GenerateChatCompletionAsync(messages);
        }

        public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
        {
            var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await adapter!.GenerateChatCompletionByAgentAsync(messages);
        }

        public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages, InferenceAiAdapterType eIAInferenceAdapterType)
        {
            var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await adapter!.GenerateChatCompletionByAgentSimpleRagAsync(messages);
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text, InferenceAiAdapterType eIAInferenceAdapterType)
        {
            var adapter = _adapterFactory.CreateAdapter(eIAInferenceAdapterType);
            return await adapter!.GenerateEmbeddingAsync(text);
        }
    }
}
