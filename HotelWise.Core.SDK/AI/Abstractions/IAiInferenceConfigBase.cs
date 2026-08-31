namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato base de configuração de inferência IA.
/// Define endpoint, chave, modelos de chat e embeddings compartilhados
/// pelos provedores (OpenAI, Azure OpenAI, Mistral, Groq, Ollama, etc.).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IAiInferenceConfigBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IAiInferenceConfigBase
{
    /// <summary>
    /// Chave de API do provedor de inferência.
    /// </summary>
    string ApiKey { get; set; }

    /// <summary>
    /// Endpoint HTTP do serviço de chat/completions.
    /// </summary>
    string Endpoint { get; set; }

    /// <summary>
    /// Identificador do modelo de chat.
    /// </summary>
    string ModelId { get; set; }

    /// <summary>
    /// Identificador da organização (quando o provedor exige, ex.: OpenAI).
    /// </summary>
    string? OrgId { get; set; }

    /// <summary>
    /// Endpoint HTTP do serviço de embeddings.
    /// </summary>
    string EndpointEmbeddings { get; set; }

    /// <summary>
    /// Identificador do modelo de embeddings.
    /// </summary>
    string ModelIdEmbeddings { get; set; }
}
