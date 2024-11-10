using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Dto
{
    public class VectorStoreSettingsDto : IVectorStoreSettingsDto
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public bool Https { get; set; }

        public string ApiKey { get; set; } = string.Empty;

        public int? Timeout { get; set; }

        public string HostEmbeddings { get; set; } = string.Empty;
        public string ApiKeyEmbeddings { get; set; } = string.Empty;
    }
}
 