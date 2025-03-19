using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces
{
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