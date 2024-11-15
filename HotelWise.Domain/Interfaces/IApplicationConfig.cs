

using HotelWise.Domain.Dto.AppConfig;

namespace HotelWise.Domain.Interfaces
{
    public interface IApplicationConfig
    {
        AzureAISearchConfig AzureAISearchConfig { get; }
        AzureCosmosDBConfig AzureCosmosDBMongoDBConfig { get; }
        AzureCosmosDBConfig AzureCosmosDBNoSQLConfig { get; }
        AzureOpenAIConfig AzureOpenAIConfig { get; }
        AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig { get; }
        OpenAIConfig OpenAIConfig { get; }
        OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig { get; }
        QdrantConfig QdrantConfig { get; }
        RagConfig RagConfig { get; }
        RedisConfig RedisConfig { get; }
        WeaviateConfig WeaviateConfig { get; }

        MistralApiConfig MistralApiConfig { get; }
        GroqApiConfig GroqApiConfig { get; }

    }
}