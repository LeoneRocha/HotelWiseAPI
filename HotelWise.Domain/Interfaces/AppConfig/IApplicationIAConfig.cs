using HotelWise.Domain.Dto.AppConfig.Rag;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Shim em cópia (configs/enums do host ≠ tipos Core durante a migração).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IApplicationIAConfig.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
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
}
