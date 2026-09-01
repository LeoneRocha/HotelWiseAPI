using System.Text.Json.Serialization;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de serviço de embeddings suportados no pipeline RAG.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AIEmbeddingServiceType
{
    DefaultEmbeddings = (int)SchEnums.AIEmbeddingServiceType.DefaultEmbeddings,
    OllamaEmbeddings = (int)SchEnums.AIEmbeddingServiceType.OllamaEmbeddings,
    AzureOpenAIEmbeddings = (int)SchEnums.AIEmbeddingServiceType.AzureOpenAIEmbeddings,
    OpenAIEmbeddings = (int)SchEnums.AIEmbeddingServiceType.OpenAIEmbeddings,
    MistralApiEmbeddings = (int)SchEnums.AIEmbeddingServiceType.MistralApiEmbeddings,
    CohereEmbeddings = (int)SchEnums.AIEmbeddingServiceType.CohereEmbeddings,
    HuggingFaceEmbeddings = (int)SchEnums.AIEmbeddingServiceType.HuggingFaceEmbeddings,
    SemanticKernel = (int)SchEnums.AIEmbeddingServiceType.SemanticKernel,
    SemanticKernelEmbeddings = (int)SchEnums.AIEmbeddingServiceType.SemanticKernelEmbeddings,
    OllamaAdapter = (int)SchEnums.AIEmbeddingServiceType.OllamaAdapter,
    SentenceTransformersEmbeddings = (int)SchEnums.AIEmbeddingServiceType.SentenceTransformersEmbeddings,
}
