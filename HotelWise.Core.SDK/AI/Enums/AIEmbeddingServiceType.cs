using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Tipos de serviço de embeddings suportados no pipeline RAG.
/// Usado em <see cref="Abstractions.IRagConfig"/> e na resolução de configuração
/// via <see cref="Abstractions.IApplicationIAConfig.GetEmbeddingServiceConfig"/>.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AIEmbeddingServiceType
{
    /// <summary>
    /// Embeddings padrão (fallback da aplicação).
    /// </summary>
    DefaultEmbeddings,

    /// <summary>
    /// Embeddings via Ollama.
    /// </summary>
    OllamaEmbeddings,

    /// <summary>
    /// Embeddings via Azure OpenAI.
    /// </summary>
    AzureOpenAIEmbeddings,

    /// <summary>
    /// Embeddings via OpenAI.
    /// </summary>
    OpenAIEmbeddings,

    /// <summary>
    /// Embeddings via Mistral API.
    /// </summary>
    MistralApiEmbeddings,

    /// <summary>
    /// Embeddings via Cohere.
    /// </summary>
    CohereEmbeddings,

    /// <summary>
    /// Embeddings via Hugging Face.
    /// </summary>
    HuggingFaceEmbeddings,

    /// <summary>
    /// Embeddings orquestrados pelo Semantic Kernel.
    /// </summary>
    SemanticKernel,

    /// <summary>
    /// Gerador de embeddings do Semantic Kernel.
    /// </summary>
    SemanticKernelEmbeddings,

    /// <summary>
    /// Embeddings via adapter Ollama.
    /// </summary>
    OllamaAdapter,

    /// <summary>
    /// Embeddings via Sentence Transformers.
    /// </summary>
    SentenceTransformersEmbeddings
}
