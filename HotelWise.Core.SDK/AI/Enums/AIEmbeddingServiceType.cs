using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

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
