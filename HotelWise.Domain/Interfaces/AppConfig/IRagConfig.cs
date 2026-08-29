using HotelWise.Domain.Dto.AppConfig.Rag;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces.AppConfig
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Shim em cópia (enums/DTOs do host ≠ tipos Core durante a migração).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IRagConfig.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IRagConfig
    {
        AIChatServiceType AIChatServiceApi { get; }
        AIEmbeddingServiceType AIEmbeddingServiceApi { get; }
        AIChatServiceType AIChatServiceAdapter { get; }
        AIEmbeddingServiceType AIEmbeddingServiceAdapter { get; }
        bool BuildCollection { get; }
        string VectorStoreCollectionPrefixName { get; }
        int VectorStoreDimensions { get; }
        int DataLoadingBatchSize { get; }
        int DataLoadingBetweenBatchDelayInMilliseconds { get; }
        string[]? PdfFilePaths { get; }
        VectorStoreType VectorStoreType { get; }
        SearchSettings SearchSettings { get; }

        InferenceAiAdapterType GetAInferenceAdapterType();
    }
}
