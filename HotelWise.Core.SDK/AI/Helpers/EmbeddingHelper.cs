namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitário de conversão de embeddings para tipos usados pelo vector store
/// (<see cref="ReadOnlyMemory{T}"/> de <see cref="float"/>).
/// </summary>
/// <example>
/// <code>
/// float[] vetor = await inference.GenerateEmbeddingAsync("texto");
/// ReadOnlyMemory&lt;float&gt; memory = EmbeddingHelper.ConvertToReadOnlyMemory(vetor);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Helpers.EmbeddingHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class EmbeddingHelper
{
    /// <summary>
    /// Converte um array de floats em <see cref="ReadOnlyMemory{T}"/> para uso em buscas vetoriais.
    /// </summary>
    /// <param name="embeddings">Array de floats do embedding.</param>
    /// <returns>Memória somente leitura envolvendo o array.</returns>
    /// <example>
    /// <code>
    /// var memory = EmbeddingHelper.ConvertToReadOnlyMemory(new float[] { 0.1f, 0.2f });
    /// </code>
    /// </example>
    public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings)
    {
        return new ReadOnlyMemory<float>(embeddings);
    }
}
