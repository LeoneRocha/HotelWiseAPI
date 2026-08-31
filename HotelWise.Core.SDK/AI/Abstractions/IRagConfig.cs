namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração RAG (Retrieval-Augmented Generation).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IRagConfig
    : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig
{
}
