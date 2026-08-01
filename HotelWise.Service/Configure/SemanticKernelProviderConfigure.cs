using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Dto.AppConfig.Rag;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.AppConfig;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using CommunityToolkit.VectorData.Qdrant;
using Microsoft.SemanticKernel.TextGeneration;
using OllamaSharp;

namespace HotelWise.Service.Configure
{
    public static class SemanticKernelProviderConfigure
    {
        public static void SetupSemanticKernelProvider(IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = addApplicationConfig(services, configuration);

            addRagConfig(services, configuration);

            var builder = Kernel.CreateBuilder();

            // Register the kernel with the dependency injection container
            // and add Chat Completion and Text Embedding Generation services.

            addQdrantVectorStoreToBuilder(appConfig, builder);

            addAIServices(appConfig, builder);

            var kernel = builder.Build();

            addServicesDependecies(services, kernel, appConfig);
        }

        private static void addRagConfig(IServiceCollection services, IConfiguration configuration)
        {
            // Bind the PolicyConfig section of appsettings.json to the PolicyConfig class
            var appSettingsValue = new RagConfig();

            var configValue = ConfigurationAppSettingsHelper.GetRagConfig(configuration);

            new ConfigureFromConfigurationOptions<RagConfig>(configValue).Configure(appSettingsValue);
            // Register the PolicyConfig instance as a singleton
            services.AddSingleton<IRagConfig>(appSettingsValue);
        }

        private static ApplicationIAConfig addApplicationConfig(IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = new ApplicationIAConfig(configuration);

            // Register the PolicyConfig instance as a singleton
            services.AddSingleton<IApplicationIAConfig>(appConfig);
            return appConfig;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]

        private static void addAIServices(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
            var aiServiceType = appConfig?.RagConfig?.AIChatServiceApi ?? AIChatServiceType.MistralApi;

#pragma warning disable SKEXP0070
            switch (aiServiceType)
            {
                case AIChatServiceType.AzureOpenAI:
                    break;
                case AIChatServiceType.OpenAI:
                    break;
                case AIChatServiceType.GroqApi:
                    break;
                case AIChatServiceType.Default:
                case AIChatServiceType.MistralApi:
                    addMistral(appConfig!, builder);
                    break;
                case AIChatServiceType.Anthropic:
                    break;
                case AIChatServiceType.Cohere:
                    break;
                case AIChatServiceType.Ollama:
                case AIChatServiceType.OllamaAdapter:
                    addOllama(appConfig!, builder);
                    break;
                case AIChatServiceType.LlamaCpp:
                    break;
                case AIChatServiceType.HuggingFace:
                    break;
                default:
                    break;
            }
#pragma warning restore SKEXP0070
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addMistral(IApplicationIAConfig appConfig, IKernelBuilder builder)
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

            builder.Services.AddTransient((serviceProvider) => {
                return new Kernel(serviceProvider);
            });

#pragma warning restore SKEXP0070
        }

        private static void addOllama(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
            var modelConfig = appConfig.OllamaConfig;
            EnsureConfigured(
                modelConfig.Endpoint,
                "ApplicationIAConfig:AIServices:OllamaApi:Endpoint",
                "ApplicationIAConfig__AIServices__OllamaApi__Endpoint");
#pragma warning disable SKEXP0070
            builder.AddOllamaChatCompletion(modelId: modelConfig.ModelId, endpoint: new Uri(modelConfig.Endpoint));
            builder.AddOllamaEmbeddingGenerator(modelId: modelConfig.ModelIdEmbeddings, endpoint: new Uri(modelConfig.EndpointEmbeddings));

            builder.Services.AddTransient((serviceProvider) => { return new Kernel(serviceProvider); });

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

        private static void addServicesDependecies(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
        {
            services.AddKernel();
            services.AddSingleton(kernel);

            addKernelVectorStoreToServiceCollection(services, kernel);

            addTextEmbeddingGenerationService(services, kernel, configuration);

            addChatCompletionService(services, kernel);
        }

        private static void addChatCompletionService(IServiceCollection services, Kernel kernel)
        {
            #region ChatCompletionService

            IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();
            services.AddSingleton(chatService);

            #endregion ChatCompletionService
        }

        private static void addKernelVectorStoreToServiceCollection(IServiceCollection services, Kernel kernel)
        {
            #region VectorStores

            VectorStore vectorStore = kernel.GetRequiredService<VectorStore>();
            services.AddSingleton(vectorStore);

            #endregion VectorStores
        }

        private static void addTextEmbeddingGenerationService(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
        {
            #region TextEmbeddingGenerationService

            var typeAIEmbeddingService = configuration.RagConfig.AIEmbeddingServiceApi;
            switch (typeAIEmbeddingService)
            {
                case AIEmbeddingServiceType.DefaultEmbeddings:
                    addDefaultTextEmbeddingGenerationService(services, kernel);
                    break;
                case AIEmbeddingServiceType.AzureOpenAIEmbeddings:
                    break;
                case AIEmbeddingServiceType.OpenAIEmbeddings:
                    break;
                case AIEmbeddingServiceType.MistralApiEmbeddings:
                    addDefaultTextEmbeddingGenerationService(services, kernel);
                    break;
                case AIEmbeddingServiceType.CohereEmbeddings:
                    break;
                case AIEmbeddingServiceType.HuggingFaceEmbeddings:
                    break;
                case AIEmbeddingServiceType.OllamaEmbeddings:
                    addOllamaTextEmbeddingGenerationService(services, configuration);
                    break;
                case AIEmbeddingServiceType.SentenceTransformersEmbeddings:
                    break;
                default:
                    break;
            }

            #endregion TextEmbeddingGenerationService
        }
        private static void addOllamaTextEmbeddingGenerationService(IServiceCollection services, IApplicationIAConfig configuration)
        {
#pragma warning disable SKEXP0001
            var ollamaAdapter = new OllamaAdapter(configuration);
            var ollamaClient = ollamaAdapter.GetClientEmbedding();
#pragma warning disable SKEXP0070
            Func<IServiceProvider, OllamaTextGenerationService> factory = (serviceProvider) =>
            {
                return new OllamaTextGenerationService(ollamaClient, serviceProvider.GetService<ILoggerFactory>());
            };
            services.AddSingleton<ITextGenerationService>(factory);
#pragma warning restore SKEXP0070 
#pragma warning restore SKEXP0001
        }

        private static void addDefaultTextEmbeddingGenerationService(IServiceCollection services, Kernel kernel)
        {
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
                kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
            services.AddSingleton(embeddingGenerator);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addQdrantVectorStoreToBuilder(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
#pragma warning disable SKEXP0020
            #region Vector Store

            var qdrantConfig = appConfig.QdrantConfig;

            builder.Services.AddQdrantCollection<ulong, HotelVector>(
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
            #endregion Vector Store
#pragma warning restore SKEXP0020
        }
    }
}