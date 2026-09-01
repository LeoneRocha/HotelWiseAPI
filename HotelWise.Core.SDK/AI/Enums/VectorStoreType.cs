using System.Text.Json.Serialization;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de vector store suportados pelo pipeline RAG.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Enums.VectorStoreType", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Enums.VectorStoreType em SmartCoreHub.Core.SDK.")]
public enum VectorStoreType
{
    InMemory = (int)SchEnums.VectorStoreType.InMemory,
    AzureAISearch = (int)SchEnums.VectorStoreType.AzureAISearch,
    AzureCosmosDBMongoDB = (int)SchEnums.VectorStoreType.AzureCosmosDBMongoDB,
    AzureCosmosDBNoSQL = (int)SchEnums.VectorStoreType.AzureCosmosDBNoSQL,
    Qdrant = (int)SchEnums.VectorStoreType.Qdrant,
    Redis = (int)SchEnums.VectorStoreType.Redis,
    Weaviate = (int)SchEnums.VectorStoreType.Weaviate,
    Pinecone = (int)SchEnums.VectorStoreType.Pinecone,
    Chroma = (int)SchEnums.VectorStoreType.Chroma,
    Milvus = (int)SchEnums.VectorStoreType.Milvus,
    PostgresVectorStore = (int)SchEnums.VectorStoreType.PostgresVectorStore,
}
