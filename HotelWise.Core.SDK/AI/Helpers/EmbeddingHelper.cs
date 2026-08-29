namespace HotelWise.Core.SDK.AI.Helpers;

/// <summary>
/// Conversão de embeddings.
/// </summary>
public static class EmbeddingHelper
{
    public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings)
    {
        return new ReadOnlyMemory<float>(embeddings);
    }
}
