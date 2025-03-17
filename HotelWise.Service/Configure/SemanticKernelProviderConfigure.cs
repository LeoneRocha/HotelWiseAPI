using HotelWise.Domain.AI.Adapter;
using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Embeddings;
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

            addVectorStores(appConfig, builder);

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
            var aiServiceType = appConfig?.RagConfig?.AIChatService ?? AIChatServiceType.MistralApi;

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
            var mistralEmbeddings = appConfig.MistralApíEmbeddingsConfig;
            var mistral = appConfig.MistralApiConfig;
#pragma warning disable SKEXP0070
            // Optional; for targeting specific services within Semantic Kernel    httpClient: new HttpClient() // Optional; for customizing HTTP client , endpoint: new Uri("YOUR_ENDPOINT"), serviceId: "SERVICE_ID"
            builder.AddMistralChatCompletion(modelId: mistral.ModelId, apiKey: mistral.ApiKey);
            builder.AddMistralTextEmbeddingGeneration(modelId: mistral.ModelId, apiKey: mistral.ApiKey);
#pragma warning restore SKEXP0070
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addOllama(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
            var modelConfig = appConfig.OllamaConfig;
#pragma warning disable SKEXP0070
            //https://ollama.com/library/llama3.2
            builder.AddOllamaChatCompletion(modelId: modelConfig.ModelId, endpoint: new Uri(modelConfig.Endpoint));
            //https://ollama.com/library/nomic-embed-text
            builder.AddOllamaTextEmbeddingGeneration(modelId: modelConfig.ModelIdEmbeddings, endpoint: new Uri(modelConfig.EndpointEmbeddings));
             
            builder.Services.AddTransient((serviceProvider) => { return new Kernel(serviceProvider); });

#pragma warning restore SKEXP0070
        }

        private static void addServicesDependecies(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
        {
            services.AddKernel();
            services.AddSingleton(kernel);

            addVectorStores(services, kernel);

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

        private static void addVectorStores(IServiceCollection services, Kernel kernel)
        {
            #region VectorStores

            IVectorStore vectorStore = kernel.GetRequiredService<IVectorStore>();
            services.AddSingleton(vectorStore);

            #endregion VectorStores
        }

        private static void addTextEmbeddingGenerationService(IServiceCollection services, Kernel kernel, IApplicationIAConfig configuration)
        {
            #region TextEmbeddingGenerationService

            var typeAIEmbeddingService = configuration.RagConfig.AIEmbeddingService;
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
                    addOllamaTextEmbeddingGenerationService(services, configuration, kernel);
                    break;
                case AIEmbeddingServiceType.SentenceTransformersEmbeddings:
                    break;
                default:
                    break;
            }

            #endregion TextEmbeddingGenerationService
        }
        private static void addOllamaTextEmbeddingGenerationService(IServiceCollection services, IApplicationIAConfig configuration, Kernel kernel)
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
#pragma warning disable SKEXP0001

            ITextEmbeddingGenerationService embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
            services.AddSingleton(embeddingService);

#pragma warning restore SKEXP0001
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addVectorStores(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
#pragma warning disable SKEXP0020
            #region Vector Store

            var qdrantConfig = appConfig.QdrantConfig;

            builder.AddQdrantVectorStoreRecordCollection<ulong, HotelVector>(appConfig.RagConfig.VectorStoreCollectionPrefixName, qdrantConfig.Host, qdrantConfig.Port, qdrantConfig.Https, qdrantConfig.ApiKey);

            builder.AddQdrantVectorStore(qdrantConfig.Host, qdrantConfig.Port, qdrantConfig.Https, qdrantConfig.ApiKey, options: new QdrantVectorStoreOptions { HasNamedVectors = true });
            #endregion Vector Store
#pragma warning restore SKEXP0020
        }
    }
}