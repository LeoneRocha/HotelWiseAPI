namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO contendo os resultados consolidados da sincronização em lote de hotéis na base vetorial.
/// </summary>
public class HotelVectorSyncResultDto
{
    /// <summary>
    /// Total de hotéis encontrados para sincronização.
    /// </summary>
    public int TotalHotels { get; set; }

    /// <summary>
    /// Quantidade de hotéis sincronizados com sucesso no vetor.
    /// </summary>
    public int SynchronizedCount { get; set; }

    /// <summary>
    /// Quantidade de hotéis que falharam durante a sincronização.
    /// </summary>
    public int FailedCount { get; set; }

    /// <summary>
    /// Indica se todos os hotéis foram processados (sincronizados ou com erro registrado).
    /// </summary>
    public bool AllProcessed { get; set; }

    /// <summary>
    /// Lista de mensagens de erro detalhadas por hotel em caso de falhas parciais ou totais.
    /// </summary>
    public List<string> Errors { get; set; } = new();
}
