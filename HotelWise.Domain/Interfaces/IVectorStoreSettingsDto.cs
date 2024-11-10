namespace HotelWise.Domain.Interfaces
{
    public interface IVectorStoreSettingsDto
    {
        string ApiKey { get; set; }
        string Host { get; set; }
        bool Https { get; set; }
        int Port { get; set; }
        int? Timeout { get; set; }
        string HostEmbeddings { get; set; }
        string ApiKeyEmbeddings { get; set; }
    }
}