

using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces
{
    public interface IApplicationIAConfig
    {
        RagConfig RagConfig { get; }

        #region AIServices  
        AzureOpenAIConfig AzureOpenAIConfig { get; }
        AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig { get; }
        MistralApiConfig MistralApiConfig { get; }
        MistralApíEmbeddingsConfig MistralApíEmbeddingsConfig { get; }
        GroqApiConfig GroqApiConfig { get; }
        OllamaConfig OllamaConfig { get; }

        #endregion AIServices

        #region VectorStores

        AzureAISearchConfig AzureAISearchConfig { get; }
        AzureCosmosDBConfig AzureCosmosDBMongoDBConfig { get; }
        AzureCosmosDBConfig AzureCosmosDBNoSQLConfig { get; }
        OpenAIConfig OpenAIConfig { get; }
        OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig { get; }
        QdrantConfig QdrantConfig { get; }
        RedisConfig RedisConfig { get; }
        WeaviateConfig WeaviateConfig { get; }

        #endregion VectorStores
         
        object GetChatServiceConfig(AIChatServiceType serviceType);
        object GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType);

        object? GetVectorStoreConfig(VectorStoreType storeType);
    }
}