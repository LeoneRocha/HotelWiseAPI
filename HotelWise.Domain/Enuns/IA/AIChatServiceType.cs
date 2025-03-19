using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AIChatServiceType
    {
        Default, 
        SemanticKernel,
        AzureOpenAI,
        OpenAI,
        GroqApi,
        MistralApi,
        Anthropic,
        Cohere,
        Ollama,
        OllamaAdapter,
        LlamaCpp,
        HuggingFace,
    }
}