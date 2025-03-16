using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{ 
    /// <summary>
    /// Enumerator for Vector Store types
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VectorStoreType
    {
        InMemory,
        AzureAISearch,
        AzureCosmosDBMongoDB,
        AzureCosmosDBNoSQL,
        Qdrant,
        Redis,
        Weaviate,
        Pinecone,
        Chroma,
        Milvus,
        PostgresVectorStore
    }
}
