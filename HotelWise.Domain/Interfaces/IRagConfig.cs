using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Enuns.IA;

namespace HotelWise.Domain.Interfaces
{
    public interface IRagConfig
    {
        AIChatServiceType AIChatService { get; }
        AIEmbeddingServiceType AIEmbeddingService { get; }
        bool BuildCollection { get; }
        string CollectionName { get; }
        int DataLoadingBatchSize { get; }
        int DataLoadingBetweenBatchDelayInMilliseconds { get; }
        string[]? PdfFilePaths { get; }
        VectorStoreType VectorStoreType { get; }
        SearchSettings SearchSettings { get; } 
    }
}