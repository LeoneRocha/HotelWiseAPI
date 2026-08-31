using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de vector store suportados pelo pipeline RAG.
/// Usado em <see cref="Abstractions.IRagConfig.VectorStoreType"/> e na resolução
/// de configuração via <see cref="Abstractions.IApplicationIAConfig.GetVectorStoreConfig"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Enums.VectorStoreType. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public enum VectorStoreType
{
    /// <summary>
    /// Armazenamento em memória (sem persistência externa).
    /// </summary>
    InMemory,

    /// <summary>
    /// Azure AI Search como vector store.
    /// </summary>
    AzureAISearch,

    /// <summary>
    /// Azure Cosmos DB com API MongoDB.
    /// </summary>
    AzureCosmosDBMongoDB,

    /// <summary>
    /// Azure Cosmos DB com API NoSQL.
    /// </summary>
    AzureCosmosDBNoSQL,

    /// <summary>
    /// Qdrant como vector store.
    /// </summary>
    Qdrant,

    /// <summary>
    /// Redis como vector store.
    /// </summary>
    Redis,

    /// <summary>
    /// Weaviate como vector store.
    /// </summary>
    Weaviate,

    /// <summary>
    /// Pinecone como vector store.
    /// </summary>
    Pinecone,

    /// <summary>
    /// Chroma como vector store.
    /// </summary>
    Chroma,

    /// <summary>
    /// Milvus como vector store.
    /// </summary>
    Milvus,

    /// <summary>
    /// PostgreSQL com extensão de vetores.
    /// </summary>
    PostgresVectorStore
}
