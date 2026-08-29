using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Enums.AIChatServiceType.", error: false, DiagnosticId = "HW_CORE_SDK_AI")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AIChatServiceType
    {
        Default, SemanticKernel, AzureOpenAI, OpenAI, GroqApi, MistralApi, Anthropic, Cohere, Ollama, OllamaAdapter, LlamaCpp, HuggingFace,
    }
}
