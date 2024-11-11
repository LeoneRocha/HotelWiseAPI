namespace HotelWise.Domain.Interfaces
{
    public interface IRagConfig
    {
        string AIChatService { get; set; }
        string AIEmbeddingService { get; set; }
        bool BuildCollection { get; set; }
        string CollectionName { get; set; }
        int DataLoadingBatchSize { get; set; }
        int DataLoadingBetweenBatchDelayInMilliseconds { get; set; }
        string[]? PdfFilePaths { get; set; }
        string VectorStoreType { get; set; }
    }
}