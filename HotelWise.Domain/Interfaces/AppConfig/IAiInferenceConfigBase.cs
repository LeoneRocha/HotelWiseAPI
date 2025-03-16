namespace HotelWise.Domain.Interfaces.AppConfig
{
    public interface IAiInferenceConfigBase
    {
        string ApiKey { get; set; }
        string Endpoint { get; set; }
        public string ModelId { get; set; }
        string? OrgId { get; set; }
        string EndpointEmbeddings { get; set; }
        string ModelIdEmbeddings { get; set; }
    }
}