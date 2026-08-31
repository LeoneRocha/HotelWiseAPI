using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using SchEnums = SmartCoreHub.Core.SDK.Domain.AI.Enums;
using SchConfig = SmartCoreHub.Core.SDK.Domain.AI.Configuration;

namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configuração RAG da aplicação (seção <c>ApplicationIAConfig:Rag</c>).
/// Composição sobre o tipo sealed SCH — zero lógica própria; enums HW espelhados via cast.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.RagConfig. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public sealed class RagConfig : IRagConfig
{
    private readonly SchConfig.RagConfig _inner;
    private SearchSettings _searchSettings;

    /// <summary>
    /// Nome completo da seção de configuração no appsettings.
    /// </summary>
    public const string ConfigSectionName = SchConfig.RagConfig.ConfigSectionName;

    /// <summary>
    /// Instância sealed SCH (bridge para adapters/runtime SCH).
    /// </summary>
    internal SchConfig.RagConfig Inner => _inner;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RagConfig"/>.
    /// </summary>
    public RagConfig() : this(new SchConfig.RagConfig())
    {
    }

    internal RagConfig(SchConfig.RagConfig inner)
    {
        _inner = inner ?? new SchConfig.RagConfig();
        _searchSettings = _inner.SearchSettings as SearchSettings
            ?? SearchSettings.FromSch(_inner.SearchSettings);
        _inner.SearchSettings = _searchSettings;
    }

    internal static RagConfig FromSch(SchConfig.RagConfig inner) =>
        inner is null ? new RagConfig() : new RagConfig(inner);

    /// <summary>Provedor de API de chat IA padrão.</summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceApi
    {
        get => (AIChatServiceType)(int)_inner.AIChatServiceApi;
        set => _inner.AIChatServiceApi = (SchEnums.AIChatServiceType)(int)value;
    }

    /// <summary>Provedor de API de embedding IA padrão.</summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceApi
    {
        get => (AIEmbeddingServiceType)(int)_inner.AIEmbeddingServiceApi;
        set => _inner.AIEmbeddingServiceApi = (SchEnums.AIEmbeddingServiceType)(int)value;
    }

    /// <summary>Adapter de serviço de chat IA padrão.</summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIChatServiceType AIChatServiceAdapter
    {
        get => (AIChatServiceType)(int)_inner.AIChatServiceAdapter;
        set => _inner.AIChatServiceAdapter = (SchEnums.AIChatServiceType)(int)value;
    }

    /// <summary>Adapter de serviço de embedding IA padrão.</summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AIEmbeddingServiceType AIEmbeddingServiceAdapter
    {
        get => (AIEmbeddingServiceType)(int)_inner.AIEmbeddingServiceAdapter;
        set => _inner.AIEmbeddingServiceAdapter = (SchEnums.AIEmbeddingServiceType)(int)value;
    }

    /// <summary>Indica se a coleção deve ser criada automaticamente na inicialização.</summary>
    [Required]
    public bool BuildCollection
    {
        get => _inner.BuildCollection;
        set => _inner.BuildCollection = value;
    }

    /// <summary>Prefixo para nome de coleções no vector store.</summary>
    [Required]
    public string VectorStoreCollectionPrefixName
    {
        get => _inner.VectorStoreCollectionPrefixName;
        set => _inner.VectorStoreCollectionPrefixName = value;
    }

    /// <summary>Dimensões do vetor de embedding.</summary>
    [Required]
    public int VectorStoreDimensions
    {
        get => _inner.VectorStoreDimensions;
        set => _inner.VectorStoreDimensions = value;
    }

    /// <summary>Tamanho do lote para carga de dados vetoriais.</summary>
    [Required]
    public int DataLoadingBatchSize
    {
        get => _inner.DataLoadingBatchSize;
        set => _inner.DataLoadingBatchSize = value;
    }

    /// <summary>Atraso em milissegundos entre lotes de carga de dados.</summary>
    [Required]
    public int DataLoadingBetweenBatchDelayInMilliseconds
    {
        get => _inner.DataLoadingBetweenBatchDelayInMilliseconds;
        set => _inner.DataLoadingBetweenBatchDelayInMilliseconds = value;
    }

    /// <summary>Caminhos de arquivos PDF para ingestão RAG.</summary>
    [Required]
    public string[]? PdfFilePaths
    {
        get => _inner.PdfFilePaths;
        set => _inner.PdfFilePaths = value;
    }

    /// <summary>Tipo do provedor de vector store configurado.</summary>
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VectorStoreType VectorStoreType
    {
        get => (VectorStoreType)(int)_inner.VectorStoreType;
        set => _inner.VectorStoreType = (SchEnums.VectorStoreType)(int)value;
    }

    /// <summary>Configurações de busca e thresholds de similaridade.</summary>
    public SearchSettings SearchSettings
    {
        get => _searchSettings;
        set
        {
            _searchSettings = value ?? new SearchSettings();
            _inner.SearchSettings = _searchSettings;
        }
    }

    /// <summary>
    /// Obtém o tipo de adapter de inferência configurado.
    /// </summary>
    /// <returns>Tipo de adapter de inferência.</returns>
    public InferenceAiAdapterType GetAInferenceAdapterType() =>
        (InferenceAiAdapterType)(int)_inner.GetAInferenceAdapterType();
}
