namespace HotelWise.Domain.Helpers
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Helpers.EmbeddingHelper.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public static class EmbeddingHelper
    {
        public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings) =>
            HotelWise.Core.SDK.AI.Helpers.EmbeddingHelper.ConvertToReadOnlyMemory(embeddings);
    }
}
