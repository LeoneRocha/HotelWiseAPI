

using HotelWise.Domain.Dto.AppConfig;

namespace HotelWise.Domain.Interfaces
{
    public interface IApplicationConfig
    {
        RagConfig RagConfig { get; }

        #region AIServices  
        AzureOpenAIConfig AzureOpenAIConfig { get; }
        AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig { get; }
        MistralApiConfig MistralApiConfig { get; }
        MistralApíEmbeddingsConfig MistralApíEmbeddingsConfig { get; }
        GroqApiConfig GroqApiConfig { get; }

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
    }
}