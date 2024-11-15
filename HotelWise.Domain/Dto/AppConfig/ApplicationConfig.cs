using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Domain.Dto.AppConfig
{
    /// <summary>
    /// Helper class to load all configuration settings for the VectorStoreRAG project.
    /// </summary>
    public sealed class ApplicationConfig : IApplicationConfig
    {
        #region FIELDS
        private readonly AzureOpenAIConfig _azureOpenAIConfig = new();
        private readonly AzureOpenAIEmbeddingsConfig _azureOpenAIEmbeddingsConfig = new();
        private readonly OpenAIConfig _openAIConfig = new();
        private readonly OpenAIEmbeddingsConfig _openAIEmbeddingsConfig = new();
        private readonly RagConfig _ragConfig = new();
        private readonly AzureAISearchConfig _azureAISearchConfig = new();
        private readonly AzureCosmosDBConfig _azureCosmosDBMongoDBConfig = new();
        private readonly AzureCosmosDBConfig _azureCosmosDBNoSQLConfig = new();
        private readonly QdrantConfig _qdrantConfig = new();
        private readonly RedisConfig _redisConfig = new();
        private readonly WeaviateConfig _weaviateConfig = new();
        private readonly MistralApiConfig _mistralApiConfig = new();
        private readonly GroqApiConfig _groqApiConfig = new();
        #endregion FIELDS

        #region PROPERTIES
        public AzureOpenAIConfig AzureOpenAIConfig => this._azureOpenAIConfig;
        public AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddingsConfig => this._azureOpenAIEmbeddingsConfig;
        public OpenAIConfig OpenAIConfig => this._openAIConfig;
        public OpenAIEmbeddingsConfig OpenAIEmbeddingsConfig => this._openAIEmbeddingsConfig;
        public RagConfig RagConfig => this._ragConfig;
        public AzureAISearchConfig AzureAISearchConfig => this._azureAISearchConfig;
        public AzureCosmosDBConfig AzureCosmosDBMongoDBConfig => this._azureCosmosDBMongoDBConfig;
        public AzureCosmosDBConfig AzureCosmosDBNoSQLConfig => this._azureCosmosDBNoSQLConfig;
        public QdrantConfig QdrantConfig => this._qdrantConfig;
        public RedisConfig RedisConfig => this._redisConfig;
        public WeaviateConfig WeaviateConfig => this._weaviateConfig;
        public MistralApiConfig MistralApiConfig => this._mistralApiConfig;
        public GroqApiConfig GroqApiConfig => this._groqApiConfig;
        #endregion PROPERTIES

        public ApplicationConfig(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection(RagConfig.ConfigSectionName).Bind(this._ragConfig);
            loadIAServices(configurationManager);
            loadStores(configurationManager);
        }

        private void loadIAServices(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection($"AIServices:{AzureOpenAIConfig.ConfigSectionName}").Bind(this._azureOpenAIConfig);
            configurationManager.GetRequiredSection($"AIServices:{AzureOpenAIEmbeddingsConfig.ConfigSectionName}").Bind(this._azureOpenAIEmbeddingsConfig);
            configurationManager.GetRequiredSection($"AIServices:{OpenAIConfig.ConfigSectionName}").Bind(this._openAIConfig);
            configurationManager.GetRequiredSection($"AIServices:{OpenAIEmbeddingsConfig.ConfigSectionName}").Bind(this._openAIEmbeddingsConfig);
            configurationManager.GetRequiredSection($"AIServices:{MistralApiConfig.ConfigSectionName}").Bind(this._mistralApiConfig);
            configurationManager.GetRequiredSection($"AIServices:{GroqApiConfig.ConfigSectionName}").Bind(this._groqApiConfig);
        }

        private void loadStores(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection($"VectorStores:{AzureAISearchConfig.ConfigSectionName}").Bind(this._azureAISearchConfig);
            configurationManager.GetRequiredSection($"VectorStores:{AzureCosmosDBConfig.MongoDBConfigSectionName}").Bind(this._azureCosmosDBMongoDBConfig);
            configurationManager.GetRequiredSection($"VectorStores:{AzureCosmosDBConfig.NoSQLConfigSectionName}").Bind(this._azureCosmosDBNoSQLConfig);
            configurationManager.GetRequiredSection($"VectorStores:{QdrantConfig.ConfigSectionName}").Bind(this._qdrantConfig);
            configurationManager.GetRequiredSection($"VectorStores:{RedisConfig.ConfigSectionName}").Bind(this._redisConfig);
            configurationManager.GetRequiredSection($"VectorStores:{WeaviateConfig.ConfigSectionName}").Bind(this._weaviateConfig);
        }
    }
}