using System.Text;

namespace HotelWise.Service.Prompts;

/// <summary>
/// Gerador de prompts StayMate. Prática recomendada: uma única Instruction (role Agent)
/// — sem System duplicado — o adapter compõe o system canônico a partir do Agent.
/// </summary>
public static class StayMatePromptGenerator
{
    /// <summary>
    /// Cria a instrução única do agente de busca de hotéis (persona + regras + formato).
    /// </summary>
    public static PromptMessageVO CreateHotelAgentPrompt()
    {
        var message = new StringBuilder()
            .AppendLine("Você é StayMate, um assistente amigável e especializado em turismo.")
            .AppendLine("Sua tarefa é avaliar o contexto recuperado (hotéis) e confirmar de forma criativa que os resultados atendem ao filtro, sem justificativas de exclusão.")
            .AppendLine()
            .AppendLine("Diretrizes:")
            .AppendLine("- Use **somente** fatos e IDs presentes no contexto recuperado.")
            .AppendLine("- Liste apenas hotéis que atendam exatamente à consulta.")
            .AppendLine("- Inclua IDs ocultos em comentários HTML: <!-- ID-Hotel: 1234 -->")
            .AppendLine("- Não invente IDs, cidades ou detalhes ausentes do contexto.")
            .AppendLine("- Se não houver hotéis válidos, diga educadamente e sugira refinar a busca.")
            .AppendLine("- Responda em português brasileiro (pt-BR), em Markdown.")
            .AppendLine()
            .AppendLine("Exemplo:")
            .AppendLine("---")
            .AppendLine("### Opções de Hotéis para Você")
            .AppendLine()
            .AppendLine("Olá! Selecionei opções que atendem ao que você pediu:")
            .AppendLine()
            .AppendLine("<!-- ID-Hotel: 1234 -->")
            .AppendLine("<!-- ID-Hotel: 5678 -->")
            .AppendLine()
            .AppendLine("_Estou aqui para ajudar no que precisar._")
            .AppendLine("---")
            .ToString();

        return new PromptMessageVO
        {
            RoleType = RoleAiPromptsType.Agent,
            AgentName = "StayMate",
            Content = message
        };
    }

#pragma warning disable S1133
    /// <summary>
    /// Mantido por compatibilidade; delega para <see cref="CreateHotelAgentPrompt"/>
    /// (evita segundo system no pipeline).
    /// </summary>
    [Obsolete("Use CreateHotelAgentPrompt — uma única instruction canônica.")]
    public static PromptMessageVO CreateHotelSystemPrompt() => CreateHotelAgentPrompt();
#pragma warning restore S1133
}
