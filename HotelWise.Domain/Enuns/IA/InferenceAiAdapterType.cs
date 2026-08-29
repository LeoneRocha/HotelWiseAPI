using System.ComponentModel;

namespace HotelWise.Domain.Enuns.IA
{
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Enums.InferenceAiAdapterType.", error: false, DiagnosticId = "HW_CORE_SDK_AI")]
    public enum InferenceAiAdapterType
    {
        [Description("GroqApi")] GroqApi = 0,
        [Description("Mistral")] Mistral = 1,
        [Description("Ollama")] Ollama = 2,
        [Description("SemanticKernel")] SemanticKernel = 3,
    }
}
