using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Resposta do assistente conversacional retornada por <see cref="Abstractions.IAssistantService"/>.
/// Representa uma mensagem tipada por papel (role) no diálogo.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantResponse. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AskAssistantResponse
{
    /// <summary>
    /// Papel da mensagem na conversa (user, assistant, system, etc.).
    /// </summary>
    public RoleAiPromptsType Role { get; set; }

    /// <summary>
    /// Conteúdo textual da resposta.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Token de sessão/conversa associado à resposta.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Solicitação ao assistente conversacional.
/// Enviada a <see cref="Abstractions.IAssistantService.AskAssistant"/> com a mensagem do usuário.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.AskAssistantRequest. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AskAssistantRequest
{
    /// <summary>
    /// Papel da solicitação; sempre <see cref="RoleAiPromptsType.User"/>.
    /// </summary>
    public RoleAiPromptsType Role { get; } = RoleAiPromptsType.User;

    /// <summary>
    /// Mensagem enviada pelo usuário ao assistente.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Token de sessão/conversa para correlacionar o diálogo.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}
