#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using SchAbstractions = SmartCoreHub.Core.SDK.Domain.AI.Abstractions;
using SchAdapters = SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters;
using SchConfig = SmartCoreHub.Core.SDK.Domain.AI.Configuration;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Bridge HW <see cref="IApplicationIAConfig"/> → SCH (prefer <see cref="ApplicationIAConfig.Inner"/>).
/// </summary>
internal static class ApplicationIAConfigSchBridge
{
    public static SchAbstractions.IApplicationIAConfig ToSch(IApplicationIAConfig applicationConfig)
    {
        if (applicationConfig is ApplicationIAConfig concrete)
            return concrete.Inner;
        return new Adapter(applicationConfig);
    }

    private sealed class Adapter : SchAbstractions.IApplicationIAConfig
    {
        private readonly IApplicationIAConfig _hw;

        public Adapter(IApplicationIAConfig hw) => _hw = hw;

        public SchConfig.RagConfig RagConfig => _hw.RagConfig.Inner;
        public SchConfig.AzureOpenAIConfig AzureOpenAIConfig => _hw.AzureOpenAIConfig;
        public SchConfig.AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig => _hw.AzureOpenAIEmbeddingsConfig;
        public SchConfig.MistralApiConfig MistralApiConfig => _hw.MistralApiConfig;
        public SchConfig.MistralApiEmbeddingsConfig MistralApiEmbeddingsConfig => _hw.MistralApiEmbeddingsConfig;
        public SchConfig.GroqApiConfig GroqApiConfig => _hw.GroqApiConfig;
        public SchConfig.OllamaConfig OllamaConfig => _hw.OllamaConfig;
        public SchConfig.AzureAISearchConfig AzureAISearchConfig => _hw.AzureAISearchConfig;
        public SchConfig.AzureCosmosDBConfig AzureCosmosDBMongoDBConfig => _hw.AzureCosmosDBMongoDBConfig;
        public SchConfig.AzureCosmosDBConfig AzureCosmosDBNoSQLConfig => _hw.AzureCosmosDBNoSQLConfig;
        public SchConfig.OpenAIConfig OpenAIConfig => _hw.OpenAIConfig;
        public SchConfig.OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig => _hw.OpenAIEmbeddingsConfig;
        public SchConfig.QdrantConfig QdrantConfig => _hw.QdrantConfig;
        public SchConfig.RedisConfig RedisConfig => _hw.RedisConfig;
        public SchConfig.WeaviateConfig WeaviateConfig => _hw.WeaviateConfig;

        public SchAbstractions.IAiInferenceConfigBase GetChatServiceConfig(SchEnums.AIChatServiceType serviceType) =>
            _hw.GetChatServiceConfig((Enums.AIChatServiceType)(int)serviceType);

        public SchAbstractions.IAiInferenceConfigBase GetChatServiceConfig() =>
            _hw.GetChatServiceConfig();

        public SchAbstractions.IAiInferenceConfigBase GetEmbeddingServiceConfig(SchEnums.AIEmbeddingServiceType embeddingType) =>
            _hw.GetEmbeddingServiceConfig((Enums.AIEmbeddingServiceType)(int)embeddingType);

        public object? GetVectorStoreConfig(SchEnums.VectorStoreType storeType) =>
            _hw.GetVectorStoreConfig((Enums.VectorStoreType)(int)storeType);
    }
}

/// <summary>
/// Adapter de inferência via Groq API — casca sobre SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.AI.Adapters.GroqApiAdapter. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class GroqApiAdapter : IAIInferenceAdapter
{
    private readonly SchAdapters.GroqApiAdapter _inner;

    public GroqApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _inner = new SchAdapters.GroqApiAdapter(ApplicationIAConfigSchBridge.ToSch(applicationConfig));
    }

    public Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionAsync(messages);

    public Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentAsync(messages);

    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        _inner.GenerateEmbeddingAsync(text);

    public Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        _inner.GenerateChatCompletionByAgentSimpleRagAsync(messages);
}
#endif
