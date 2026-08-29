using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Enums.AIEmbeddingServiceType.", error: false, DiagnosticId = "HW_CORE_SDK_AI")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AIEmbeddingServiceType
    {
        DefaultEmbeddings, OllamaEmbeddings, AzureOpenAIEmbeddings, OpenAIEmbeddings, MistralApiEmbeddings, CohereEmbeddings, HuggingFaceEmbeddings, SemanticKernel, SemanticKernelEmbeddings, OllamaAdapter, SentenceTransformersEmbeddings
    }
}
