using System.ComponentModel;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de adapter de inferência LLM disponíveis na fábrica.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Enums.InferenceAiAdapterType. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
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
