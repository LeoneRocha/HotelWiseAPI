using System.ComponentModel;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de adapter de inferência LLM disponíveis na fábrica
/// <see cref="Abstractions.IAIInferenceAdapterFactory"/>.
/// </summary>
public enum InferenceAiAdapterType
{
    /// <summary>
    /// Adapter Groq API.
    /// </summary>
    [Description("GroqApi")]
    GroqApi = 0,

    /// <summary>
    /// Adapter Mistral API.
    /// </summary>
    [Description("Mistral")]
    Mistral = 1,

    /// <summary>
    /// Adapter Ollama (inferência local).
    /// </summary>
    [Description("Ollama")]
    Ollama = 2,

    /// <summary>
    /// Adapter Semantic Kernel (orquestração multi-provedor).
    /// </summary>
    [Description("SemanticKernel")]
    SemanticKernel = 3,
}
