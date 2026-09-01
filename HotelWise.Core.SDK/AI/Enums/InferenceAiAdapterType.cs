using System.ComponentModel;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de adapter de inferência LLM disponíveis na fábrica.
/// </summary>
public enum InferenceAiAdapterType
{
    [Description("GroqApi")]
    GroqApi = (int)SchEnums.InferenceAiAdapterType.GroqApi,

    [Description("Mistral")]
    Mistral = (int)SchEnums.InferenceAiAdapterType.Mistral,

    [Description("Ollama")]
    Ollama = (int)SchEnums.InferenceAiAdapterType.Ollama,

    [Description("SemanticKernel")]
    SemanticKernel = (int)SchEnums.InferenceAiAdapterType.SemanticKernel,
}
