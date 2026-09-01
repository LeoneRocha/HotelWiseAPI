
namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Utilitário de conversão de embeddings para tipos usados pelo vector store.
/// </summary>
public static class EmbeddingHelper
{
    /// <summary>
    /// Converte um array de floats em <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    /// <param name="embeddings">Array de floats com os embeddings.</param>
    /// <returns>Memória somente leitura dos embeddings.</returns>
    public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings) =>
        SmartCoreHub.Core.SDK.Service.AI.Helpers.EmbeddingHelper.ConvertToReadOnlyMemory(embeddings);
}
