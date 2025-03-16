using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Dto.AppConfig
{  
    public sealed class RagConfig : IRagConfig
    {
        public const string ConfigSectionName = "Rag";

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AIChatServiceType AIChatService { get; set; } = AIChatServiceType.OpenAI;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AIEmbeddingServiceType AIEmbeddingService { get; set; } = AIEmbeddingServiceType.OpenAIEmbeddings;

        [Required]
        public bool BuildCollection { get; set; } = true;

        [Required]
        public string CollectionName { get; set; } = string.Empty;

        [Required]
        public int DataLoadingBatchSize { get; set; } = 2;

        [Required]
        public int DataLoadingBetweenBatchDelayInMilliseconds { get; set; } = 0;

        [Required]
        public string[]? PdfFilePaths { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VectorStoreType VectorStoreType { get; set; } = VectorStoreType.InMemory;

        public SearchSettings SearchSettings { get; set; } = new();

        //// Propriedades de compatibilidade para IRagConfig (se a interface ainda requer strings)
        //string IRagConfig.AIChatService => AIChatService.ToString();
        //string IRagConfig.AIEmbeddingService => AIEmbeddingService.ToString();
        //string IRagConfig.VectorStoreType => VectorStoreType.ToString();
    }
}