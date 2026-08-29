using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato agregado de configuração de IA da aplicação.
/// </summary>
public interface IApplicationIAConfig
{
    RagConfig RagConfig { get; }

    AzureOpenAIConfig AzureOpenAIConfig { get; }
    AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig { get; }
    MistralApiConfig MistralApiConfig { get; }
    MistralApiEmbeddingsConfig MistralApiEmbeddingsConfig { get; }
    GroqApiConfig GroqApiConfig { get; }
    OllamaConfig OllamaConfig { get; }

    AzureAISearchConfig AzureAISearchConfig { get; }
    AzureCosmosDBConfig AzureCosmosDBMongoDBConfig { get; }
    AzureCosmosDBConfig AzureCosmosDBNoSQLConfig { get; }
    OpenAIConfig OpenAIConfig { get; }
    OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig { get; }
    QdrantConfig QdrantConfig { get; }
    RedisConfig RedisConfig { get; }
    WeaviateConfig WeaviateConfig { get; }

    IAiInferenceConfigBase GetChatServiceConfig(AIChatServiceType serviceType);
    IAiInferenceConfigBase GetChatServiceConfig();
    IAiInferenceConfigBase GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType);
    object? GetVectorStoreConfig(VectorStoreType storeType);
}
