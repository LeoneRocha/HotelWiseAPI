using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Adapter de inferência LLM (chat e embeddings).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAIInferenceAdapter
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapter
{
}

/// <summary>
/// Fábrica de adapters de inferência LLM.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAIInferenceAdapterFactory
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceAdapterFactory
{
}

/// <summary>
/// Serviço de orquestração de inferência.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAIInferenceService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAIInferenceService
{
}

/// <summary>
/// Serviço de assistente conversacional voltado ao usuário final.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAssistantService
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAssistantService
{
}
