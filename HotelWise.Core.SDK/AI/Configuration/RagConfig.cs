using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configuração RAG da aplicação (seção <c>ApplicationIAConfig:Rag</c>).
/// Define provedores de chat/embeddings, vector store, dimensões e parâmetros
/// de carga usados na indexação e na busca semântica do pipeline RAG.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.RagConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public sealed class RagConfig : IRagConfig
{
    /// <summary>
    /// Nome completo da seção de configuração no appsettings.
    /// </summary>
    public const string ConfigSectionName = "ApplicationIAConfig:Rag";

    /// <summary>
    /// Tipo do serviço de chat usado na API.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceApi { get; set; } = AIChatServiceType.Default;

    /// <summary>
    /// Tipo do serviço de embeddings usado na API.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceApi { get; set; } = AIEmbeddingServiceType.OpenAIEmbeddings;

    /// <summary>
    /// Tipo do serviço de chat usado pelos adapters.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceAdapter { get; set; } = AIChatServiceType.Default;

    /// <summary>
    /// Tipo do serviço de embeddings usado pelos adapters.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceAdapter { get; set; } = AIEmbeddingServiceType.DefaultEmbeddings;

    /// <summary>
    /// Indica se a coleção do vector store deve ser criada/atualizada na carga.
    /// </summary>
    [Required]
    public bool BuildCollection { get; set; } = true;

    /// <summary>
    /// Prefixo do nome da coleção no vector store.
    /// </summary>
    [Required]
    public string VectorStoreCollectionPrefixName { get; set; } = string.Empty;

    /// <summary>
    /// Dimensão dos vetores de embedding armazenados.
    /// </summary>
    [Required]
    public int VectorStoreDimensions { get; set; } = 1024;

    /// <summary>
    /// Tamanho do lote na carga de dados para o vector store.
    /// </summary>
    [Required]
    public int DataLoadingBatchSize { get; set; } = 2;

    /// <summary>
    /// Atraso em milissegundos entre lotes de carga de dados.
    /// </summary>
    [Required]
    public int DataLoadingBetweenBatchDelayInMilliseconds { get; set; }

    /// <summary>
    /// Caminhos de arquivos PDF usados como fonte de documentos para o RAG.
    /// </summary>
    [Required]
    public string[]? PdfFilePaths { get; set; }

    /// <summary>
    /// Tipo do vector store ativo.
    /// </summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VectorStoreType VectorStoreType { get; set; } = VectorStoreType.InMemory;

    /// <summary>
    /// Configurações auxiliares de busca vetorial/RAG.
    /// </summary>
    public SearchSettings SearchSettings { get; set; } = new();

    /// <summary>
    /// Resolve o tipo de adapter de inferência a partir de <see cref="AIChatServiceAdapter"/>.
    /// </summary>
    /// <returns>Tipo do adapter de inferência correspondente.</returns>
    public InferenceAiAdapterType GetAInferenceAdapterType()
    {
        switch (AIChatServiceAdapter)
        {
            case AIChatServiceType.Default:
            case AIChatServiceType.SemanticKernel:
                return InferenceAiAdapterType.SemanticKernel;
            case AIChatServiceType.GroqApi:
            case AIChatServiceType.MistralApi:
                return InferenceAiAdapterType.GroqApi;
            case AIChatServiceType.Ollama:
            case AIChatServiceType.OllamaAdapter:
                return InferenceAiAdapterType.Ollama;
            default:
                return InferenceAiAdapterType.SemanticKernel;
        }
    }
}
