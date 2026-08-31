using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Extensions;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial embutido em prompts RAG.
/// Carrega a chave e o texto recuperado do vector store para enriquecer a inferência.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class DataVectorVO
{
    /// <summary>
    /// Identificador do vetor/registro de origem no store.
    /// </summary>
    public string KeyVector { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo textual do fragmento recuperado.
    /// </summary>
    public string DataVector { get; set; } = string.Empty;
}

/// <summary>
/// Mensagem de prompt para adapters de inferência (<see cref="Abstractions.IAIInferenceAdapter"/>).
/// Representa um turno do histórico com papel, conteúdo, contexto RAG opcional e contagem de tokens.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class PromptMessageVO
{
    /// <summary>
    /// Fragmentos de contexto RAG associados à mensagem (quando o papel é Context).
    /// </summary>
    public DataVectorVO[] DataContextRag { get; set; } = Array.Empty<DataVectorVO>();

    /// <summary>
    /// Tipo de papel da mensagem no histórico.
    /// </summary>
    public RoleAiPromptsType RoleType { get; set; }

    /// <summary>
    /// Descrição textual do papel (derivada de <see cref="RoleType"/>).
    /// </summary>
    public string Role => RoleType.GetDescription();

    /// <summary>
    /// Conteúdo textual da mensagem.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Nome do agente, quando <see cref="RoleType"/> é <see cref="RoleAiPromptsType.Agent"/>.
    /// </summary>
    public string AgentName { get; set; } = string.Empty;

    /// <summary>
    /// Contagem aproximada de tokens do conteúdo ou dos fragmentos RAG.
    /// </summary>
    public int TokenCount
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(Content))
            {
                return HotelWise.Core.SDK.AI.Helpers.TokenCounterHelper.CountTokens(Content);
            }
            if (DataContextRag != null && DataContextRag.Length > 0)
            {
                HotelWise.Core.SDK.AI.Helpers.TokenCounterHelper.CalculateDataVectorLength(DataContextRag);
            }
            return 0;
        }
    }

    /// <summary>
    /// Comprimento em caracteres de <see cref="Content"/>.
    /// </summary>
    public int ContentLenght => Content.Length;
}
