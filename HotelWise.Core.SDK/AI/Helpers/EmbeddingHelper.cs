namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitário de conversão de embeddings para tipos usados pelo vector store.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Helpers.EmbeddingHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class EmbeddingHelper
{
    public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings) =>
        SmartCoreHub.Core.SDK.Service.AI.Helpers.EmbeddingHelper.ConvertToReadOnlyMemory(embeddings);
}
