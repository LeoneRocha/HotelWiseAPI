using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de configuração RAG (Retrieval-Augmented Generation).
/// Define provedores de chat/embeddings, vector store, dimensões e parâmetros
/// de carga usados na indexação e na busca semântica.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IRagConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IRagConfig
{
    /// <summary>
    /// Tipo do serviço de chat usado na API (provedor de inferência HTTP).
    /// </summary>
    AIChatServiceType AIChatServiceApi { get; }

    /// <summary>
    /// Tipo do serviço de embeddings usado na API.
    /// </summary>
    AIEmbeddingServiceType AIEmbeddingServiceApi { get; }

    /// <summary>
    /// Tipo do serviço de chat usado pelos adapters (ex.: Semantic Kernel, Ollama).
    /// </summary>
    AIChatServiceType AIChatServiceAdapter { get; }

    /// <summary>
    /// Tipo do serviço de embeddings usado pelos adapters.
    /// </summary>
    AIEmbeddingServiceType AIEmbeddingServiceAdapter { get; }

    /// <summary>
    /// Indica se a coleção do vector store deve ser criada/atualizada na carga.
    /// </summary>
    bool BuildCollection { get; }

    /// <summary>
    /// Prefixo do nome da coleção no vector store.
    /// </summary>
    string VectorStoreCollectionPrefixName { get; }

    /// <summary>
    /// Dimensão dos vetores de embedding armazenados.
    /// </summary>
    int VectorStoreDimensions { get; }

    /// <summary>
    /// Tamanho do lote na carga de dados para o vector store.
    /// </summary>
    int DataLoadingBatchSize { get; }

    /// <summary>
    /// Atraso em milissegundos entre lotes de carga de dados.
    /// </summary>
    int DataLoadingBetweenBatchDelayInMilliseconds { get; }

    /// <summary>
    /// Caminhos de arquivos PDF usados como fonte de documentos para o RAG, quando aplicável.
    /// </summary>
    string[]? PdfFilePaths { get; }

    /// <summary>
    /// Tipo do vector store ativo (Qdrant, Redis, InMemory, etc.).
    /// </summary>
    VectorStoreType VectorStoreType { get; }

    /// <summary>
    /// Configurações auxiliares de busca vetorial/RAG.
    /// </summary>
    SearchSettings SearchSettings { get; }

    /// <summary>
    /// Resolve o tipo de adapter de inferência a partir de <see cref="AIChatServiceAdapter"/>.
    /// </summary>
    /// <returns>Tipo do adapter de inferência correspondente.</returns>
    InferenceAiAdapterType GetAInferenceAdapterType();
}
