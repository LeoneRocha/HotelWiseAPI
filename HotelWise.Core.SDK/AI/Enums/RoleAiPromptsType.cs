using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HotelWise.Core.SDK.AI.Enums;

/// <summary>
/// Papéis (roles) das mensagens no histórico de prompts de IA.
/// Alinhados aos papéis de chat completion e extensões do pipeline RAG
/// (Agent para instruções de agente; Context para fragmentos recuperados).
/// </summary>
public enum RoleAiPromptsType
{
    /// <summary>
    /// Mensagem de sistema (instruções globais ao modelo).
    /// </summary>
    [JsonPropertyName("system")]
    [Description("system")]
    System = 1,

    /// <summary>
    /// Mensagem do usuário.
    /// </summary>
    [JsonPropertyName("user")]
    [Description("user")]
    User = 2,

    /// <summary>
    /// Mensagem do assistente (respostas anteriores do modelo).
    /// </summary>
    [JsonPropertyName("assistant")]
    [Description("assistant")]
    Assistant = 3,

    /// <summary>
    /// Configuração de agente (instruções e nome do agente).
    /// </summary>
    [JsonPropertyName("agent")]
    [Description("agent")]
    Agent = 4,

    /// <summary>
    /// Contexto RAG (fragmentos vetoriais recuperados).
    /// </summary>
    [JsonPropertyName("Context")]
    [Description("Context")]
    Context = 5
}
