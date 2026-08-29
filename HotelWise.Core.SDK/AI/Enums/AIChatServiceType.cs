using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de serviço de chat/completion suportados no pipeline de IA.
/// Usado em <see cref="Abstractions.IRagConfig"/> e na resolução de configuração
/// via <see cref="Abstractions.IApplicationIAConfig.GetChatServiceConfig"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AIChatServiceType
{
    /// <summary>
    /// Serviço de chat padrão (fallback da aplicação).
    /// </summary>
    Default,

    /// <summary>
    /// Chat via Semantic Kernel.
    /// </summary>
    SemanticKernel,

    /// <summary>
    /// Chat via Azure OpenAI.
    /// </summary>
    AzureOpenAI,

    /// <summary>
    /// Chat via OpenAI.
    /// </summary>
    OpenAI,

    /// <summary>
    /// Chat via Groq API.
    /// </summary>
    GroqApi,

    /// <summary>
    /// Chat via Mistral API.
    /// </summary>
    MistralApi,

    /// <summary>
    /// Chat via Anthropic.
    /// </summary>
    Anthropic,

    /// <summary>
    /// Chat via Cohere.
    /// </summary>
    Cohere,

    /// <summary>
    /// Chat via Ollama.
    /// </summary>
    Ollama,

    /// <summary>
    /// Chat via adapter Ollama dedicado.
    /// </summary>
    OllamaAdapter,

    /// <summary>
    /// Chat via llama.cpp.
    /// </summary>
    LlamaCpp,

    /// <summary>
    /// Chat via Hugging Face.
    /// </summary>
    HuggingFace,
}
