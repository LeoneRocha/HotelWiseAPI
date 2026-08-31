#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using Microsoft.Extensions.Configuration;
using SchAI = SmartCoreHub.Core.SDK.Domain.AI;
using SchConfig = SmartCoreHub.Core.SDK.Domain.AI.Configuration;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Carrega e agrega configurações de IA a partir de <see cref="IConfiguration"/>.
/// Composição sobre o tipo sealed SCH — propriedades HW tipadas; <see cref="Inner"/> para bridges.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.ApplicationIAConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public sealed class ApplicationIAConfig : IApplicationIAConfig
{
    /// <summary>
    /// Nome da seção raiz de configuração de IA no appsettings.
    /// </summary>
    public const string ConfigSectionName = SchConfig.ApplicationIAConfig.ConfigSectionName;

    private readonly SchConfig.ApplicationIAConfig _inner;
    private readonly RagConfig _ragConfig;
    private readonly AzureOpenAIConfig _azureOpenAIConfig;
    private readonly AzureOpenAIEmbeddingsConfig _azureOpenAIEmbeddingsConfig;
    private readonly OpenAIConfig _openAIConfig;
    private readonly OpenAIEmbeddingsConfig _openAIEmbeddingsConfig;
    private readonly AzureAISearchConfig _azureAISearchConfig;
    private readonly AzureCosmosDBConfig _azureCosmosDBMongoDBConfig;
    private readonly AzureCosmosDBConfig _azureCosmosDBNoSQLConfig;
    private readonly QdrantConfig _qdrantConfig;
    private readonly RedisConfig _redisConfig;
    private readonly WeaviateConfig _weaviateConfig;
    private readonly MistralApiConfig _mistralApiConfig;
    private readonly GroqApiConfig _groqApiConfig;
    private readonly MistralApiEmbeddingsConfig _mistralApiEmbeddingsConfig;
    private readonly OllamaConfig _ollamaConfig;

    /// <summary>
    /// Instância sealed SCH (bridge para adapters/runtime SCH).
    /// </summary>
    internal SchConfig.ApplicationIAConfig Inner => _inner;

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
        _inner = new SchConfig.ApplicationIAConfig(configurationManager);
        _ragConfig = RagConfig.FromSch(_inner.RagConfig);
        _azureOpenAIConfig = Wrap(_inner.AzureOpenAIConfig, () => new AzureOpenAIConfig());
        _azureOpenAIEmbeddingsConfig = Wrap(_inner.AzureOpenAIEmbeddingsConfig, () => new AzureOpenAIEmbeddingsConfig());
        _openAIConfig = Wrap(_inner.OpenAIConfig, () => new OpenAIConfig());
        _openAIEmbeddingsConfig = Wrap(_inner.OpenAIEmbeddingsConfig, () => new OpenAIEmbeddingsConfig());
        _azureAISearchConfig = Wrap(_inner.AzureAISearchConfig, () => new AzureAISearchConfig());
        _azureCosmosDBMongoDBConfig = Wrap(_inner.AzureCosmosDBMongoDBConfig, () => new AzureCosmosDBConfig());
        _azureCosmosDBNoSQLConfig = Wrap(_inner.AzureCosmosDBNoSQLConfig, () => new AzureCosmosDBConfig());
        _qdrantConfig = Wrap(_inner.QdrantConfig, () => new QdrantConfig());
        _redisConfig = Wrap(_inner.RedisConfig, () => new RedisConfig());
        _weaviateConfig = Wrap(_inner.WeaviateConfig, () => new WeaviateConfig());
        _mistralApiConfig = Wrap(_inner.MistralApiConfig, () => new MistralApiConfig());
        _groqApiConfig = Wrap(_inner.GroqApiConfig, () => new GroqApiConfig());
        _mistralApiEmbeddingsConfig = Wrap(_inner.MistralApiEmbeddingsConfig, () => new MistralApiEmbeddingsConfig());
        _ollamaConfig = Wrap(_inner.OllamaConfig, () => new OllamaConfig());
    }

    public IAiInferenceConfigBase GetChatServiceConfig(AIChatServiceType serviceType) =>
        WrapInference(_inner.GetChatServiceConfig((SchAI.Enums.AIChatServiceType)(int)serviceType));

    public IAiInferenceConfigBase GetChatServiceConfig() =>
        WrapInference(_inner.GetChatServiceConfig());

    public IAiInferenceConfigBase GetEmbeddingServiceConfig(AIEmbeddingServiceType embeddingType) =>
        WrapInference(_inner.GetEmbeddingServiceConfig((SchAI.Enums.AIEmbeddingServiceType)(int)embeddingType));

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
            _ => _inner.GetVectorStoreConfig((SchAI.Enums.VectorStoreType)(int)storeType)
        };

    private IAiInferenceConfigBase WrapInference(SchAI.Abstractions.IAiInferenceConfigBase sch)
    {
        if (ReferenceEquals(sch, _inner.AzureOpenAIConfig)) return _azureOpenAIConfig;
        if (ReferenceEquals(sch, _inner.AzureOpenAIEmbeddingsConfig)) return _azureOpenAIEmbeddingsConfig;
        if (ReferenceEquals(sch, _inner.OpenAIConfig)) return _openAIConfig;
        if (ReferenceEquals(sch, _inner.OpenAIEmbeddingsConfig)) return _openAIEmbeddingsConfig;
        if (ReferenceEquals(sch, _inner.MistralApiConfig)) return _mistralApiConfig;
        if (ReferenceEquals(sch, _inner.MistralApiEmbeddingsConfig)) return _mistralApiEmbeddingsConfig;
        if (ReferenceEquals(sch, _inner.GroqApiConfig)) return _groqApiConfig;
        if (ReferenceEquals(sch, _inner.OllamaConfig)) return _ollamaConfig;
        if (sch is IAiInferenceConfigBase hw) return hw;
        throw new NotSupportedException($"Cannot wrap inference config of type {sch.GetType().FullName}");
    }

    private static THw Wrap<THw, TSch>(TSch sch, Func<THw> factory)
        where THw : TSch
        where TSch : class
    {
        if (sch is THw hw)
            return hw;
        var copy = factory();
        CopyPublicSettableProperties(sch, copy);
        return copy;
    }

    private static void CopyPublicSettableProperties(object source, object dest)
    {
        var sType = source.GetType();
        var dType = dest.GetType();
        foreach (var sp in sType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
            if (!sp.CanRead)
                continue;
            var dp = dType.GetProperty(sp.Name);
            if (dp is null || !dp.CanWrite || !dp.PropertyType.IsAssignableFrom(sp.PropertyType))
                continue;
            dp.SetValue(dest, sp.GetValue(source));
        }
    }
}
#endif
