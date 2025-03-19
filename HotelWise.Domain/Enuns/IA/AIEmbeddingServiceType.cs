using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    /// <summary>
    /// Enumerator for AI Embedding services
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AIEmbeddingServiceType
    {
        DefaultEmbeddings,
        OllamaEmbeddings,
        AzureOpenAIEmbeddings,
        OpenAIEmbeddings,
        MistralApiEmbeddings,
        CohereEmbeddings,
        HuggingFaceEmbeddings,
        SemanticKernel,
        SemanticKernelEmbeddings,
        OllamaAdapter,
        SentenceTransformersEmbeddings
    }
}
