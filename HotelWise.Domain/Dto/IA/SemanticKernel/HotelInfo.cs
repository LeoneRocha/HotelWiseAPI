namespace HotelWise.Domain.Dto.IA.SemanticKernel;

/// <summary>
/// DTO auxiliar que representa dados simplificados de identificação de hotel em plugins do Semantic Kernel.
/// </summary>
public class HotelInfo
{
    /// <summary>
    /// Identificador numérico do hotel.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Tipo de identificador ou categoria associada.
    /// </summary>
    public string IdType { get; set; } = string.Empty;
}
