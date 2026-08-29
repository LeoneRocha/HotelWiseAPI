using Microsoft.Extensions.VectorData;

namespace HotelWise.Domain.Dto.IA.SemanticKernel;

/// <summary>
/// Modelo de registro vetorial de Hotel para persistência e busca semântica em Vector Stores (Qdrant, Redis, etc.).
/// </summary>
public class HotelVector : HotelWise.Core.SDK.AI.DTO.DataVectorBase
{
    /// <summary>
    /// Nome do hotel indexado para filtragem vetorial.
    /// </summary>
    [VectorStoreData(IsIndexed = true)]
    public string HotelName { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do hotel com suporte a busca textual completa (Full-Text Search).
    /// </summary>
    [VectorStoreData(IsFullTextIndexed = true)]
    public string Description { get; set; } = string.Empty;
}
