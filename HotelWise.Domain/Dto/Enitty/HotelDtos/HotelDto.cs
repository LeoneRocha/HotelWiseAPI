using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Dto.Enitty.HotelDtos;

/// <summary>
/// DTO de transferência e resposta de Hotel, estendendo a entidade de domínio <see cref="Hotel"/> com metadados de busca semântica e vetorização.
/// </summary>
public class HotelDto : Hotel
{
    /// <summary>
    /// Indica se o hotel está indexado na base vetorial (Vector Store) para buscas semânticas.
    /// </summary>
    public bool IsHotelInVectorStore { get; set; }

    /// <summary>
    /// Pontuação de similaridade semântica (relevância) obtida na busca vetorial.
    /// </summary>
    public double Score { get; set; }
}