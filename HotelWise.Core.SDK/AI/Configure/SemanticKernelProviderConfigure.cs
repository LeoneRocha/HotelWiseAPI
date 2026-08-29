#if NET8_0_OR_GREATER
using CommunityToolkit.VectorData.Qdrant;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Adapters;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Helpers;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.TextGeneration;
using OllamaSharp;

namespace HotelWise.Core.SDK.AI.Configure;

/// <summary>
/// Configuração genérica do Semantic Kernel / vector store.
/// </summary>
public static class SemanticKernelProviderConfigure
{
    public static void SetupSemanticKernelProvider<TVector>(IServiceCollection services, IConfiguration configuration)
        where TVector : class
    {
        var appConfig = AddApplicationConfig(services, configuration);
        AddRagConfig(services, configuration);

        var builder = Kernel.CreateBuilder();
        AddQdrantVectorStoreToBuilder<TVector>(appConfig, builder);
        AddAIServices(appConfig, builder);

        var kernel = builder.Build();
        AddServicesDependencies(services, kernel, appConfig);
    }

    private static void AddRagConfig(IServiceCollection services, IConfiguration configuration)
    {
        var appSettingsValue = new RagConfig();
        var configValue = ConfigurationAppSettingsHelper.GetRagConfig(configuration);
        new ConfigureFromConfigurationOptions<RagConfig>(configValue).Configure(appSettingsValue);
        services.AddSingleton<IRagConfig>(appSettingsValue);
    }

    private static ApplicationIAConfig AddApplicationConfig(IServiceCollection services, IConfiguration configuration)
    {
        var appConfig = new ApplicationIAConfig(configuration);
        services.AddSingleton<IApplicationIAConfig>(appConfig);
        return appConfig;
    }

    private static void AddAIServices(IApplicationIAConfig appConfig, IKernelBuilder builder)
    {
        var aiServiceType = appConfig?.RagConfig?.AIChatServiceApi ?? AIChatServiceType.MistralApi;

#pragma warning disable SKEXP0070
        switch (aiServiceType)
        {
            case AIChatServiceType.Default:
            case AIChatServiceType.MistralApi:
                AddMistral(appConfig!, builder);
                break;
            case AIChatServiceType.Ollama:
            case AIChatServiceType.OllamaAdapter:
                AddOllama(appConfig!, builder);
                break;
            default:
                break;
        }
#pragma warning restore SKEXP0070
    }

    private static void AddMistral(IApplicationIAConfig appConfig, IKernelBuilder builder)
    {
#pragma warning disable SKEXP0070
        var mistral = appConfig.MistralApiConfig;
        EnsureConfigured(
            mistral.ApiKey,
            "ApplicationIAConfig:AIServices:MistralApi:ApiKey",
            "ApplicationIAConfig__AIServices__MistralApi__ApiKey");

        builder.AddMistralChatCompletion(modelId: mistral.ModelId, apiKey: mistral.ApiKey);

        var mistralEmbeddings = appConfig.MistralApiEmbeddingsConfig;
        EnsureConfigured(
            mistralEmbeddings.ApiKey,
            "ApplicationIAConfig:AIServices:MistralApiEmbeddings:ApiKey",
            "ApplicationIAConfig__AIServices__MistralApiEmbeddings__ApiKey");

        builder.AddMistralEmbeddingGenerator(modelId: mistralEmbeddings.ModelId, apiKey: mistralEmbeddings.ApiKey);
        builder.Services.AddTransient(serviceProvider => new Kernel(serviceProvider));
#pragma warning restore SKEXP0070
    }

    private static void AddOllama(IApplicationIAConfig appConfig, IKernelBuilder builder)
    {
        var modelConfig = appConfig.OllamaConfig;
        EnsureConfigured(
            modelConfig.Endpoint,
            "ApplicationIAConfig:AIServices:OllamaApi:Endpoint",
            "ApplicationIAConfig__AIServices__OllamaApi__Endpoint");
#pragma warning disable SKEXP0070
        builder.AddOllamaChatCompletion(modelId: modelConfig.ModelId, endpoint: new Uri(modelConfig.Endpoint));
        builder.AddOllamaEmbeddingGenerator(modelId: modelConfig.ModelIdEmbeddings, endpoint: new Uri(modelConfig.EndpointEmbeddings));
        builder.Services.AddTransient(serviceProvider => new Kernel(serviceProvider));
#pragma warning restore SKEXP0070
    }

    private static void EnsureConfigured(string? value, string configPath, string envVarHint)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Configuração obrigatória ausente: '{configPath}'. " +
                $"Defina no appsettings do ambiente de publicação ou na variável de ambiente '{envVarHint}'. " +
                "Sem isso a API falha no startup (HTTP 500.30 no IIS).");
        }
    }

    private static void AddServicesDependencies(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
    {
        services.AddKernel();
        services.AddSingleton(kernel);
        AddKernelVectorStoreToServiceCollection(services, kernel);
        AddTextEmbeddingGenerationService(services, kernel, configuration);
        AddChatCompletionService(services, kernel);
    }

    private static void AddChatCompletionService(IServiceCollection services, Kernel kernel)
    {
        IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();
        services.AddSingleton(chatService);
    }

    private static void AddKernelVectorStoreToServiceCollection(IServiceCollection services, Kernel kernel)
    {
        VectorStore vectorStore = kernel.GetRequiredService<VectorStore>();
        services.AddSingleton(vectorStore);
    }

    private static void AddTextEmbeddingGenerationService(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
    {
        var typeAIEmbeddingService = configuration.RagConfig.AIEmbeddingServiceApi;
        switch (typeAIEmbeddingService)
        {
            case AIEmbeddingServiceType.DefaultEmbeddings:
            case AIEmbeddingServiceType.MistralApiEmbeddings:
                AddDefaultTextEmbeddingGenerationService(services, kernel);
                break;
            case AIEmbeddingServiceType.OllamaEmbeddings:
                AddOllamaTextEmbeddingGenerationService(services, configuration);
                break;
            default:
                break;
        }
    }

    private static void AddOllamaTextEmbeddingGenerationService(IServiceCollection services, IApplicationIAConfig configuration)
    {
#pragma warning disable SKEXP0001
        var ollamaAdapter = new OllamaAdapter(configuration);
        var ollamaClient = ollamaAdapter.GetClientEmbedding();
#pragma warning disable SKEXP0070
        services.AddSingleton<ITextGenerationService>(serviceProvider =>
            new OllamaTextGenerationService(ollamaClient, serviceProvider.GetService<ILoggerFactory>()));
#pragma warning restore SKEXP0070
#pragma warning restore SKEXP0001
    }

    private static void AddDefaultTextEmbeddingGenerationService(IServiceCollection services, Kernel kernel)
    {
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
            kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
        services.AddSingleton(embeddingGenerator);
    }

    private static void AddQdrantVectorStoreToBuilder<TVector>(IApplicationIAConfig appConfig, IKernelBuilder builder)
        where TVector : class
    {
#pragma warning disable SKEXP0020
        var qdrantConfig = appConfig.QdrantConfig;
        builder.Services.AddQdrantCollection<ulong, TVector>(
            appConfig.RagConfig.VectorStoreCollectionPrefixName,
            qdrantConfig.Host,
            qdrantConfig.Port,
            qdrantConfig.Https,
            qdrantConfig.ApiKey);

        builder.Services.AddQdrantVectorStore(
            qdrantConfig.Host,
            qdrantConfig.Port,
            qdrantConfig.Https,
            qdrantConfig.ApiKey,
            options: new QdrantVectorStoreOptions { HasNamedVectors = true });
#pragma warning restore SKEXP0020
    }
}
#endif
