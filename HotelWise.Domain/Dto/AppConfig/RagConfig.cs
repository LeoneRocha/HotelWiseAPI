using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Interfaces;

namespace HotelWise.Domain.Dto.AppConfig
{
    public sealed class RagConfig : IRagConfig
    {
        public const string ConfigSectionName = "ApplicationIAConfig:Rag";

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AIChatServiceType AIChatService { get; set; } = AIChatServiceType.Default;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AIEmbeddingServiceType AIEmbeddingService { get; set; } = AIEmbeddingServiceType.OpenAIEmbeddings;

        [Required]
        public bool BuildCollection { get; set; } = true;

        [Required]
        public string VectorStoreCollectionPrefixName { get; set; } = string.Empty;
        [Required]
        public int VectorStoreDimensions { get; set; } = 1024;
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

        public InferenceAiAdapterType GetAInferenceAdapterType()
        {
            switch (AIChatService)
            {
                case AIChatServiceType.Default:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.AzureOpenAI:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.OpenAI:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.GroqApi:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.MistralApi:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.Anthropic:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.Cohere:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.Ollama:
                case AIChatServiceType.OllamaAdapter:
                    return InferenceAiAdapterType.Ollama;
                case AIChatServiceType.LlamaCpp:
                    return InferenceAiAdapterType.Mistral;
                case AIChatServiceType.HuggingFace:
                    return InferenceAiAdapterType.Mistral;
                default:
                    return InferenceAiAdapterType.Mistral;
            }
        }
    }
}