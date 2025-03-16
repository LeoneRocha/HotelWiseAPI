using HotelWise.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Domain.Dto.AppConfig
{
    /// <summary>
    /// Helper class to load all configuration settings for the VectorStoreRAG project.
    /// </summary>
    public sealed class ApplicationIAConfig : IApplicationIAConfig
    {
        public const string ConfigSectionName = "ApplicationIAConfig";
         
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
        private readonly MistralApíEmbeddingsConfig _mistralApíEmbeddingsConfig = new();
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
        public MistralApíEmbeddingsConfig MistralApíEmbeddingsConfig => this._mistralApíEmbeddingsConfig;
        public GroqApiConfig GroqApiConfig => this._groqApiConfig;

        #endregion PROPERTIES

        public ApplicationIAConfig(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection(RagConfig.ConfigSectionName).Bind(this._ragConfig);
            loadIAServices(configurationManager);
            loadStores(configurationManager);
            loadEmbeddings(configurationManager);
        }
        private void loadStores(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{AzureAISearchConfig.ConfigSectionName}").Bind(this._azureAISearchConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.MongoDBConfigSectionName}").Bind(this._azureCosmosDBMongoDBConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{AzureCosmosDBConfig.NoSQLConfigSectionName}").Bind(this._azureCosmosDBNoSQLConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{QdrantConfig.ConfigSectionName}").Bind(this._qdrantConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{RedisConfig.ConfigSectionName}").Bind(this._redisConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:VectorStores:{WeaviateConfig.ConfigSectionName}").Bind(this._weaviateConfig);
        }
        private void loadEmbeddings(IConfiguration configurationManager)
        {    
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{AzureOpenAIEmbeddingsConfig.ConfigSectionName}").Bind(this._azureOpenAIEmbeddingsConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{OpenAIEmbeddingsConfig.ConfigSectionName}").Bind(this._openAIEmbeddingsConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{MistralApíEmbeddingsConfig.ConfigSectionName}").Bind(this._mistralApíEmbeddingsConfig);
        }

        private void loadIAServices(IConfiguration configurationManager)
        {
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{AzureOpenAIConfig.ConfigSectionName}").Bind(this._azureOpenAIConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{OpenAIConfig.ConfigSectionName}").Bind(this._openAIConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{MistralApiConfig.ConfigSectionName}").Bind(this._mistralApiConfig);
            configurationManager.GetRequiredSection($"{ApplicationIAConfig.ConfigSectionName}:AIServices:{GroqApiConfig.ConfigSectionName}").Bind(this._groqApiConfig);
        } 
    }
}