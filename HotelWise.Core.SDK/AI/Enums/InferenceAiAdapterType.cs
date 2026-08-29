using System.ComponentModel;

namespace HotelWise.Core.SDK.AI.Enums;

public enum InferenceAiAdapterType
{
    [Description("GroqApi")]
    GroqApi = 0,

    [Description("Mistral")]
    Mistral = 1,

    [Description("Ollama")]
    Ollama = 2,

    [Description("SemanticKernel")]
    SemanticKernel = 3,
}
