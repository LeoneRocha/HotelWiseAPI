using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configuração RAG da aplicação.
/// </summary>
public sealed class RagConfig : IRagConfig
{
    public const string ConfigSectionName = "ApplicationIAConfig:Rag";

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceApi { get; set; } = AIChatServiceType.Default;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceApi { get; set; } = AIEmbeddingServiceType.OpenAIEmbeddings;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceAdapter { get; set; } = AIChatServiceType.Default;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceAdapter { get; set; } = AIEmbeddingServiceType.DefaultEmbeddings;

    [Required]
    public bool BuildCollection { get; set; } = true;

    [Required]
    public string VectorStoreCollectionPrefixName { get; set; } = string.Empty;

    [Required]
    public int VectorStoreDimensions { get; set; } = 1024;

    [Required]
    public int DataLoadingBatchSize { get; set; } = 2;

    [Required]
    public int DataLoadingBetweenBatchDelayInMilliseconds { get; set; }

    [Required]
    public string[]? PdfFilePaths { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VectorStoreType VectorStoreType { get; set; } = VectorStoreType.InMemory;

    public SearchSettings SearchSettings { get; set; } = new();

    public InferenceAiAdapterType GetAInferenceAdapterType()
    {
        switch (AIChatServiceAdapter)
        {
            case AIChatServiceType.Default:
            case AIChatServiceType.SemanticKernel:
                return InferenceAiAdapterType.SemanticKernel;
            case AIChatServiceType.GroqApi:
            case AIChatServiceType.MistralApi:
                return InferenceAiAdapterType.GroqApi;
            case AIChatServiceType.Ollama:
            case AIChatServiceType.OllamaAdapter:
                return InferenceAiAdapterType.Ollama;
            default:
                return InferenceAiAdapterType.SemanticKernel;
        }
    }
}
