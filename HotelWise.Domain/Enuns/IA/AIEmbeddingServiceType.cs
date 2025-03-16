﻿using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    /// <summary>
    /// Enumerator for AI Embedding services
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AIEmbeddingServiceType
    {
        AzureOpenAIEmbeddings,
        OpenAIEmbeddings,
        MistralApiEmbeddings,
        CohereEmbeddings,
        HuggingFaceEmbeddings,
        OllamaEmbeddings,
        SentenceTransformersEmbeddings
    }
}
