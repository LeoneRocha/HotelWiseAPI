using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelWise.Domain.Model.AI;

/// <summary>
/// Entidade de domínio para persistência do histórico completo de conversação de uma sessão de IA.
/// </summary>
public class ChatSessionHistory
{
    /// <summary>
    /// Identificador primário numérico autoincremento.
    /// </summary>
    [Column("Id", Order = 0)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    /// <summary>
    /// Título descritivo ou assunto resumido da sessão de conversa.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Identificador GUID único que referencia a sessão de chat.
    /// </summary>
    public string IdToken { get; set; } = string.Empty;

    /// <summary>
    /// Coleção de mensagens de prompt trocadas na sessão (serializadas em JSON pelo EF Core).
    /// </summary>
    public PromptMessageVO[] PromptMessageHistory { get; set; } = [];

    /// <summary>
    /// Quantidade total de mensagens acumuladas na sessão.
    /// </summary>
    public int CountMessages { get; set; }

    /// <summary>
    /// Quantidade total estimada de tokens consumidos pelas mensagens.
    /// </summary>
    public int TotalTokensMessage { get; set; }

    /// <summary>
    /// Data e hora de abertura ou registro da sessão.
    /// </summary>
    public DateTime SessionDateTime { get; set; }

    /// <summary>
    /// Identificador do usuário que originou a conversa (opcional).
    /// </summary>
    public long? IdUser { get; set; }
}

