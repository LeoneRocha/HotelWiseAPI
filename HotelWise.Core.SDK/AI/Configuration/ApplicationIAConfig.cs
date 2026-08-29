#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Carrega e agrega configurações de IA a partir de <see cref="IConfiguration"/>.
/// Implementa <see cref="IApplicationIAConfig"/> com seções de RAG, serviços de chat/embeddings
/// e vector stores sob a chave <see cref="ConfigSectionName"/>.
/// </summary>
public sealed class ApplicationIAConfig : IApplicationIAConfig
{
    /// <summary>
    /// Nome da seção raiz de configuração de IA no appsettings.
    /// </summary>
    public const string ConfigSectionName = "ApplicationIAConfig";

    /// <summary>
    /// Configuração RAG carregada.
    /// </summary>
    private readonly RagConfig _ragConfig = new();

    /// <summary>
    /// Configuração Azure OpenAI chat.
    /// </summary>
    private readonly AzureOpenAIConfig _azureOpenAIConfig = new();

    /// <summary>
    /// Configuração Azure OpenAI embeddings.
    /// </summary>
    private readonly AzureOpenAIEmbeddingsConfig _azureOpenAIEmbeddingsConfig = new();

    /// <summary>
    /// Configuração OpenAI chat.
    /// </summary>
    private readonly OpenAIConfig _openAIConfig = new();

    /// <summary>
    /// Configuração OpenAI embeddings.
    /// </summary>
    private readonly OpenAIEmbeddingsConfig _openAIEmbeddingsConfig = new();

    /// <summary>
    /// Configuração Azure AI Search.
    /// </summary>
    private readonly AzureAISearchConfig _azureAISearchConfig = new();

    /// <summary>
    /// Configuração Cosmos DB MongoDB.
    /// </summary>
    private readonly AzureCosmosDBConfig _azureCosmosDBMongoDBConfig = new();

    /// <summary>
    /// Configuração Cosmos DB NoSQL.
    /// </summary>
    private readonly AzureCosmosDBConfig _azureCosmosDBNoSQLConfig = new();

    /// <summary>
    /// Configuração Qdrant.
    /// </summary>
    private readonly QdrantConfig _qdrantConfig = new();

    /// <summary>
    /// Configuração Redis.
    /// </summary>
    private readonly RedisConfig _redisConfig = new();

    /// <summary>
    /// Configuração Weaviate.
    /// </summary>
    private readonly WeaviateConfig _weaviateConfig = new();

    /// <summary>
    /// Configuração Mistral API chat.
    /// </summary>
    private readonly MistralApiConfig _mistralApiConfig = new();

    /// <summary>
    /// Configuração Groq API.
    /// </summary>
    private readonly GroqApiConfig _groqApiConfig = new();

    /// <summary>
    /// Configuração Mistral API embeddings.
    /// </summary>
    private readonly MistralApiEmbeddingsConfig _mistralApiEmbeddingsConfig = new();

    /// <summary>
    /// Configuração Ollama.
    /// </summary>
    private readonly OllamaConfig _ollamaConfig = new();

    /// <summary>
    /// Configuração do serviço de chat Azure OpenAI.
    /// </summary>
    public AzureOpenAIConfig AzureOpenAIConfig => _azureOpenAIConfig;

    /// <summary>
    /// Configuração do serviço de embeddings Azure OpenAI.
    /// </summary>
    public AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig => _azureOpenAIEmbeddingsConfig;

    /// <summary>
    /// Configuração do serviço de chat OpenAI.
    /// </summary>
    public OpenAIConfig OpenAIConfig => _openAIConfig;

    /// <summary>
    /// Configuração do serviço de embeddings OpenAI.
    /// </summary>
    public OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig => _openAIEmbeddingsConfig;

    /// <summary>
    /// Configuração RAG da aplicação.
    /// </summary>
    public RagConfig RagConfig => _ragConfig;

    /// <summary>
    /// Configuração do vector store Azure AI Search.
    /// </summary>
    public AzureAISearchConfig AzureAISearchConfig => _azureAISearchConfig;

    /// <summary>
    /// Configuração do vector store Azure Cosmos DB (MongoDB).
    /// </summary>
    public AzureCosmosDBConfig AzureCosmosDBMongoDBConfig => _azureCosmosDBMongoDBConfig;

    /// <summary>
    /// Configuração do vector store Azure Cosmos DB (NoSQL).
    /// </summary>
    public AzureCosmosDBConfig AzureCosmosDBNoSQLConfig => _azureCosmosDBNoSQLConfig;

    /// <summary>
    /// Configuração do vector store Qdrant.
    /// </summary>
    public QdrantConfig QdrantConfig => _qdrantConfig;

    /// <summary>
    /// Configuração do vector store Redis.
    /// </summary>
    public RedisConfig RedisConfig => _redisConfig;

    /// <summary>
    /// Configuração do vector store Weaviate.
    /// </summary>
    public WeaviateConfig WeaviateConfig => _weaviateConfig;

    /// <summary>
    /// Configuração do serviço de chat Mistral API.
    /// </summary>
    public MistralApiConfig MistralApiConfig => _mistralApiConfig;

    /// <summary>
    /// Configuração do serviço de embeddings Mistral API.
    /// </summary>
    public MistralApiEmbeddingsConfig MistralApiEmbeddingsConfig => _mistralApiEmbeddingsConfig;

    /// <summary>
    /// Configuração do serviço de chat Groq API.
    /// </summary>
    public GroqApiConfig GroqApiConfig => _groqApiConfig;

    /// <summary>
    /// Configuração do serviço Ollama.
    /// </summary>
    public OllamaConfig OllamaConfig => _ollamaConfig;

    /// <summary>
    /// Inicializa a configuração agregada a partir de <paramref name="configurationManager"/>,
    /// vinculando seções de RAG, serviços de IA e vector stores.
    /// </summary>
    /// <param name="configurationManager">Fonte de configuração da aplicação.</param>
    public ApplicationIAConfig(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection(RagConfig.ConfigSectionName).Bind(_ragConfig);
        LoadIAServices(configurationManager);
        LoadStores(configurationManager);
        LoadEmbeddings(configurationManager);
    }

    /// <summary>
    /// Obtém a configuração de chat correspondente ao tipo de serviço informado.
    /// </summary>
    /// <param name="serviceType">Tipo do provedor de chat.</param>
    /// <returns>Configuração de inferência do provedor solicitado.</returns>
    public IAiInferenceConfigBase GetChatServiceConfig(AIChatServiceType serviceType) =>
        serviceType switch
        {
            AIChatServiceType.AzureOpenAI => _azureOpenAIConfig,
            AIChatServiceType.OpenAI => _openAIConfig,
            AIChatServiceType.MistralApi => _mistralApiConfig,
            AIChatServiceType.GroqApi => _groqApiConfig,
            AIChatServiceType.Default => _groqApiConfig,
            AIChatServiceType.OllamaAdapter => _ollamaConfig,
            _ => throw new NotImplementedException($"Configuration definition not implemented for chat service: {serviceType}")
        };

    /// <summary>
    /// Obtém a configuração de chat conforme o provedor definido em <see cref="RagConfig.AIChatServiceApi"/>.
    /// </summary>
    /// <returns>Configuração de inferência do provedor RAG ativo.</returns>
    public IAiInferenceConfigBase GetChatServiceConfig() =>
        _ragConfig.AIChatServiceApi switch
        {
            AIChatServiceType.Default => _groqApiConfig,
            AIChatServiceType.AzureOpenAI => _azureOpenAIConfig,
            AIChatServiceType.OpenAI => _openAIConfig,
            AIChatServiceType.MistralApi => _mistralApiConfig,
            AIChatServiceType.GroqApi => _groqApiConfig,
            AIChatServiceType.Ollama => _ollamaConfig,
            AIChatServiceType.OllamaAdapter => _ollamaConfig,
            _ => throw new NotImplementedException($"Configuration definition not implemented for chat service: {_ragConfig.AIChatServiceApi}")
        };

    /// <summary>
    /// Obtém a configuração de embeddings correspondente ao tipo informado.
    /// </summary>
    /// <param name="embeddingType">Tipo do provedor de embeddings.</param>
    /// <returns>Configuração de inferência do provedor de embeddings.</returns>
    public IAiInferenceConfigBase GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType) =>
        embeddingType switch
        {
            AIEmbeddingServiceType.AzureOpenAIEmbeddings => _azureOpenAIEmbeddingsConfig,
            AIEmbeddingServiceType.OpenAIEmbeddings => _openAIEmbeddingsConfig,
            AIEmbeddingServiceType.MistralApiEmbeddings => _mistralApiEmbeddingsConfig,
            AIEmbeddingServiceType.OllamaEmbeddings => _ollamaConfig,
            _ => throw new NotImplementedException($"Configuration definition not implemented for embedding service: {embeddingType}")
        };

    /// <summary>
    /// Obtém a configuração do vector store correspondente ao tipo informado.
    /// </summary>
    /// <param name="storeType">Tipo do vector store.</param>
    /// <returns>Objeto de configuração do store, ou <c>null</c> para <see cref="VectorStoreType.InMemory"/>.</returns>
    public object? GetVectorStoreConfig(VectorStoreType storeType) =>
        storeType switch
        {
            VectorStoreType.AzureAISearch => _azureAISearchConfig,
            VectorStoreType.AzureCosmosDBMongoDB => _azureCosmosDBMongoDBConfig,
            VectorStoreType.AzureCosmosDBNoSQL => _azureCosmosDBNoSQLConfig,
            VectorStoreType.Qdrant => _qdrantConfig,
            VectorStoreType.Redis => _redisConfig,
            VectorStoreType.Weaviate => _weaviateConfig,
            VectorStoreType.InMemory => null,
            _ => throw new NotImplementedException($"Configuration definition not implemented for Vector Store: {storeType}")
        };

    /// <summary>
    /// Carrega seções de vector stores a partir da configuração.
    /// </summary>
    /// <param name="configurationManager">Fonte de configuração.</param>
    private void LoadStores(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureAISearchConfig.ConfigSectionName}").Bind(_azureAISearchConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.MongoDBConfigSectionName}").Bind(_azureCosmosDBMongoDBConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.NoSQLConfigSectionName}").Bind(_azureCosmosDBNoSQLConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{QdrantConfig.ConfigSectionName}").Bind(_qdrantConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{RedisConfig.ConfigSectionName}").Bind(_redisConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{WeaviateConfig.ConfigSectionName}").Bind(_weaviateConfig);
    }

    /// <summary>
    /// Carrega seções de embeddings a partir da configuração.
    /// </summary>
    /// <param name="configurationManager">Fonte de configuração.</param>
    private void LoadEmbeddings(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{AzureOpenAIEmbeddingsConfig.ConfigSectionName}").Bind(_azureOpenAIEmbeddingsConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{OpenAIEmbeddingsConfig.ConfigSectionName}").Bind(_openAIEmbeddingsConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{MistralApiEmbeddingsConfig.ConfigSectionName}").Bind(_mistralApiEmbeddingsConfig);
    }

    /// <summary>
    /// Carrega seções de serviços de chat/IA a partir da configuração.
    /// </summary>
    /// <param name="configurationManager">Fonte de configuração.</param>
    private void LoadIAServices(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{AzureOpenAIConfig.ConfigSectionName}").Bind(_azureOpenAIConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{OpenAIConfig.ConfigSectionName}").Bind(_openAIConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{MistralApiConfig.ConfigSectionName}").Bind(_mistralApiConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{GroqApiConfig.ConfigSectionName}").Bind(_groqApiConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{OllamaConfig.ConfigSectionName}").Bind(_ollamaConfig);
    }
}
#endif
