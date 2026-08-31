using System.Text.Json.Serialization;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de serviço de chat/completion suportados no pipeline de IA.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Enums.AIChatServiceType. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public enum AIChatServiceType
{
    Default = (int)SchEnums.AIChatServiceType.Default,
    SemanticKernel = (int)SchEnums.AIChatServiceType.SemanticKernel,
    AzureOpenAI = (int)SchEnums.AIChatServiceType.AzureOpenAI,
    OpenAI = (int)SchEnums.AIChatServiceType.OpenAI,
    GroqApi = (int)SchEnums.AIChatServiceType.GroqApi,
    MistralApi = (int)SchEnums.AIChatServiceType.MistralApi,
    Anthropic = (int)SchEnums.AIChatServiceType.Anthropic,
    Cohere = (int)SchEnums.AIChatServiceType.Cohere,
    Ollama = (int)SchEnums.AIChatServiceType.Ollama,
    OllamaAdapter = (int)SchEnums.AIChatServiceType.OllamaAdapter,
    LlamaCpp = (int)SchEnums.AIChatServiceType.LlamaCpp,
    HuggingFace = (int)SchEnums.AIChatServiceType.HuggingFace,
}
