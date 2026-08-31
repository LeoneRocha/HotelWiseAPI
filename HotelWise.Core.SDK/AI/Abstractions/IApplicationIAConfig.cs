using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato agregado de configuração de IA da aplicação.
/// Centraliza provedores de chat, embeddings e vector stores usados no pipeline RAG
/// e pelos adapters de inferência (<see cref="IAIInferenceAdapter"/>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IApplicationIAConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IApplicationIAConfig
{
    /// <summary>
    /// Configuração RAG (provedores, dimensões, coleção e parâmetros de carga).
    /// </summary>
    RagConfig RagConfig { get; }

    /// <summary>
    /// Configuração do serviço de chat Azure OpenAI.
    /// </summary>
    AzureOpenAIConfig AzureOpenAIConfig { get; }

    /// <summary>
    /// Configuração do serviço de embeddings Azure OpenAI.
    /// </summary>
    AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig { get; }

    /// <summary>
    /// Configuração do serviço de chat Mistral API.
    /// </summary>
    MistralApiConfig MistralApiConfig { get; }

    /// <summary>
    /// Configuração do serviço de embeddings Mistral API.
    /// </summary>
    MistralApiEmbeddingsConfig MistralApiEmbeddingsConfig { get; }

    /// <summary>
    /// Configuração do serviço de chat Groq API.
    /// </summary>
    GroqApiConfig GroqApiConfig { get; }

    /// <summary>
    /// Configuração do serviço Ollama (chat e embeddings locais).
    /// </summary>
    OllamaConfig OllamaConfig { get; }

    /// <summary>
    /// Configuração do vector store Azure AI Search.
    /// </summary>
    AzureAISearchConfig AzureAISearchConfig { get; }

    /// <summary>
    /// Configuração do vector store Azure Cosmos DB (API MongoDB).
    /// </summary>
    AzureCosmosDBConfig AzureCosmosDBMongoDBConfig { get; }

    /// <summary>
    /// Configuração do vector store Azure Cosmos DB (API NoSQL).
    /// </summary>
    AzureCosmosDBConfig AzureCosmosDBNoSQLConfig { get; }

    /// <summary>
    /// Configuração do serviço de chat OpenAI.
    /// </summary>
    OpenAIConfig OpenAIConfig { get; }

    /// <summary>
    /// Configuração do serviço de embeddings OpenAI.
    /// </summary>
    OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig { get; }

    /// <summary>
    /// Configuração do vector store Qdrant.
    /// </summary>
    QdrantConfig QdrantConfig { get; }

    /// <summary>
    /// Configuração do vector store Redis.
    /// </summary>
    RedisConfig RedisConfig { get; }

    /// <summary>
    /// Configuração do vector store Weaviate.
    /// </summary>
    WeaviateConfig WeaviateConfig { get; }

    /// <summary>
    /// Obtém a configuração de chat correspondente ao tipo de serviço informado.
    /// </summary>
    /// <param name="serviceType">Tipo do provedor de chat.</param>
    /// <returns>Configuração de inferência do provedor solicitado.</returns>
    IAiInferenceConfigBase GetChatServiceConfig(AIChatServiceType serviceType);

    /// <summary>
    /// Obtém a configuração de chat conforme o provedor definido em <see cref="RagConfig.AIChatServiceApi"/>.
    /// </summary>
    /// <returns>Configuração de inferência do provedor RAG ativo.</returns>
    IAiInferenceConfigBase GetChatServiceConfig();

    /// <summary>
    /// Obtém a configuração de embeddings correspondente ao tipo informado.
    /// </summary>
    /// <param name="embeddingType">Tipo do provedor de embeddings.</param>
    /// <returns>Configuração de inferência do provedor de embeddings.</returns>
    IAiInferenceConfigBase GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType);

    /// <summary>
    /// Obtém a configuração do vector store correspondente ao tipo informado.
    /// </summary>
    /// <param name="storeType">Tipo do vector store.</param>
    /// <returns>Objeto de configuração do store, ou <c>null</c> para <see cref="VectorStoreType.InMemory"/>.</returns>
    object? GetVectorStoreConfig(VectorStoreType storeType);
}
