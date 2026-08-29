using HotelWise.Core.SDK.Helpers;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Dto.IA;

/// <summary>
/// DTO de transporte de histórico de sessão de chat com inteligência artificial, contendo data de atualização.
/// </summary>
public class ChatSessionHistoryDto : ChatSessionHistory
{
    /// <summary>
    /// Data e hora da última atualização dos dados da sessão no fuso local configurado.
    /// </summary>
    public DateTime UpdateDate { get; set; } = DataHelper.GetDateTimeNow();
}