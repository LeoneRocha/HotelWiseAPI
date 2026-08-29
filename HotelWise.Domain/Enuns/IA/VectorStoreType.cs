using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    [Obsolete("Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Enums.VectorStoreType.", error: false, DiagnosticId = "HW_CORE_SDK_AI")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VectorStoreType
    {
        InMemory, AzureAISearch, AzureCosmosDBMongoDB, AzureCosmosDBNoSQL, Qdrant, Redis, Weaviate, Pinecone, Chroma, Milvus, PostgresVectorStore
    }
}
