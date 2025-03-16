using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Microsoft.SemanticKernel.Embeddings;

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

            addServicesDependecies(services, kernel);
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


#pragma warning disable SKEXP0070
            //addMistral(appConfig, builder);
            // Adiciona o serviço de conclusão de chat Ollama
            addOllama(appConfig, builder);

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
            //builder.AddMistralChatCompletion(modelId: mistral.ModelId, apiKey: mistral.ApiKey);
            //builder.AddMistralTextEmbeddingGeneration(modelId: mistral.ModelId, apiKey: mistral.ApiKey);

#pragma warning restore SKEXP0070
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addOllama(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
#pragma warning disable SKEXP0070      
            //https://ollama.com/library/llama3.2
            builder.AddOllamaChatCompletion(modelId: "llama3.2", endpoint: new Uri("http://localhost:11434"));
            //https://ollama.com/library/nomic-embed-text
            builder.AddOllamaTextEmbeddingGeneration(modelId: "nomic-embed-text", endpoint: new Uri("http://localhost:11434/api/embeddings"));

#pragma warning restore SKEXP0070
        }

        private static void addServicesDependecies(IServiceCollection services, Kernel kernel)
        {
            services.AddKernel();
            services.AddSingleton(kernel);

            #region VectorStores

            IVectorStore vectorStore = kernel.GetRequiredService<IVectorStore>();
            services.AddSingleton(vectorStore);

            #endregion VectorStores

            #region TextEmbeddingGenerationService
#pragma warning disable SKEXP0001

            ITextEmbeddingGenerationService embeddingService = kernel.GetRequiredService<ITextEmbeddingGenerationService>();
            services.AddSingleton(embeddingService);

#pragma warning disable SKEXP0001
            #endregion TextEmbeddingGenerationService

            #region ChatCompletionService

            IChatCompletionService chatService = kernel.GetRequiredService<IChatCompletionService>();
            services.AddSingleton(chatService);

            #endregion ChatCompletionService
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SKEXP0070", Justification = "Usar interface para promover desacoplamento é intencional.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1859", Justification = "Usar interface para promover desacoplamento é intencional.")]
        private static void addVectorStores(IApplicationIAConfig appConfig, IKernelBuilder builder)
        {
#pragma warning disable SKEXP0020
            #region Vector Store

            var qdrantConfig = appConfig.QdrantConfig;

            builder.AddQdrantVectorStoreRecordCollection<ulong, HotelVector>(appConfig.RagConfig.CollectionName, qdrantConfig.Host, qdrantConfig.Port, qdrantConfig.Https, qdrantConfig.ApiKey);

            builder.AddQdrantVectorStore(qdrantConfig.Host, qdrantConfig.Port, qdrantConfig.Https, qdrantConfig.ApiKey, options: new QdrantVectorStoreOptions { HasNamedVectors = true });
            #endregion Vector Store
#pragma warning restore SKEXP0020
        }
    }
}