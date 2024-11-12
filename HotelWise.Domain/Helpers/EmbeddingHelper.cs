namespace HotelWise.Domain.Helpers
{
    public static class EmbeddingHelper
    {
        public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(float[] embeddings)
        {
            var resultMen = new ReadOnlyMemory<float>(embeddings);
            return resultMen;
        }
    }
}
