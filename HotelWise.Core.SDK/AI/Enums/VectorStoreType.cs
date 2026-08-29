using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

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
