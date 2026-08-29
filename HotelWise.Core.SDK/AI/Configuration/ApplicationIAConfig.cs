#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Carrega configurações de IA a partir de <see cref="IConfiguration"/>.
/// </summary>
public sealed class ApplicationIAConfig : IApplicationIAConfig
{
    public const string ConfigSectionName = "ApplicationIAConfig";

    private readonly RagConfig _ragConfig = new();
    private readonly AzureOpenAIConfig _azureOpenAIConfig = new();
    private readonly AzureOpenAIEmbeddingsConfig _azureOpenAIEmbeddingsConfig = new();
    private readonly OpenAIConfig _openAIConfig = new();
    private readonly OpenAIEmbeddingsConfig _openAIEmbeddingsConfig = new();
    private readonly AzureAISearchConfig _azureAISearchConfig = new();
    private readonly AzureCosmosDBConfig _azureCosmosDBMongoDBConfig = new();
    private readonly AzureCosmosDBConfig _azureCosmosDBNoSQLConfig = new();
    private readonly QdrantConfig _qdrantConfig = new();
    private readonly RedisConfig _redisConfig = new();
    private readonly WeaviateConfig _weaviateConfig = new();
    private readonly MistralApiConfig _mistralApiConfig = new();
    private readonly GroqApiConfig _groqApiConfig = new();
    private readonly MistralApiEmbeddingsConfig _mistralApiEmbeddingsConfig = new();
    private readonly OllamaConfig _ollamaConfig = new();

    public AzureOpenAIConfig AzureOpenAIConfig => _azureOpenAIConfig;
    public AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig => _azureOpenAIEmbeddingsConfig;
    public OpenAIConfig OpenAIConfig => _openAIConfig;
    public OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig => _openAIEmbeddingsConfig;
    public RagConfig RagConfig => _ragConfig;
    public AzureAISearchConfig AzureAISearchConfig => _azureAISearchConfig;
    public AzureCosmosDBConfig AzureCosmosDBMongoDBConfig => _azureCosmosDBMongoDBConfig;
    public AzureCosmosDBConfig AzureCosmosDBNoSQLConfig => _azureCosmosDBNoSQLConfig;
    public QdrantConfig QdrantConfig => _qdrantConfig;
    public RedisConfig RedisConfig => _redisConfig;
    public WeaviateConfig WeaviateConfig => _weaviateConfig;
    public MistralApiConfig MistralApiConfig => _mistralApiConfig;
    public MistralApiEmbeddingsConfig MistralApiEmbeddingsConfig => _mistralApiEmbeddingsConfig;
    public GroqApiConfig GroqApiConfig => _groqApiConfig;
    public OllamaConfig OllamaConfig => _ollamaConfig;

    public ApplicationIAConfig(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection(RagConfig.ConfigSectionName).Bind(_ragConfig);
        LoadIAServices(configurationManager);
        LoadStores(configurationManager);
        LoadEmbeddings(configurationManager);
    }

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

    public IAiInferenceConfigBase GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType) =>
        embeddingType switch
        {
            AIEmbeddingServiceType.AzureOpenAIEmbeddings => _azureOpenAIEmbeddingsConfig,
            AIEmbeddingServiceType.OpenAIEmbeddings => _openAIEmbeddingsConfig,
            AIEmbeddingServiceType.MistralApiEmbeddings => _mistralApiEmbeddingsConfig,
            AIEmbeddingServiceType.OllamaEmbeddings => _ollamaConfig,
            _ => throw new NotImplementedException($"Configuration definition not implemented for embedding service: {embeddingType}")
        };

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

    private void LoadStores(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureAISearchConfig.ConfigSectionName}").Bind(_azureAISearchConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.MongoDBConfigSectionName}").Bind(_azureCosmosDBMongoDBConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.NoSQLConfigSectionName}").Bind(_azureCosmosDBNoSQLConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{QdrantConfig.ConfigSectionName}").Bind(_qdrantConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{RedisConfig.ConfigSectionName}").Bind(_redisConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:VectorStores:{WeaviateConfig.ConfigSectionName}").Bind(_weaviateConfig);
    }

    private void LoadEmbeddings(IConfiguration configurationManager)
    {
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{AzureOpenAIEmbeddingsConfig.ConfigSectionName}").Bind(_azureOpenAIEmbeddingsConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{OpenAIEmbeddingsConfig.ConfigSectionName}").Bind(_openAIEmbeddingsConfig);
        configurationManager.GetRequiredSection($"{ConfigSectionName}:AIServices:{MistralApiEmbeddingsConfig.ConfigSectionName}").Bind(_mistralApiEmbeddingsConfig);
    }

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
