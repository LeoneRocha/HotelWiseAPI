using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Dto.IA.SemanticKernel;

/// <summary>
/// DTO de saída agregador do resultado de busca semântica e processamento por inteligência artificial generativa.
/// </summary>
public class HotelSemanticResult
{
    /// <summary>
    /// Resposta textual elaborada pelo modelo de linguagem baseada na consulta do usuário.
    /// </summary>
    public string PromptResultContent { get; set; } = string.Empty;

    /// <summary>
    /// Coleção de hotéis recuperados diretamente da base vetorial por proximidade de embeddings.
    /// </summary>
    public HotelDto[] HotelsVectorResult { get; set; } = [];

    /// <summary>
    /// Coleção de hotéis recomendados ou refinados após processamento analítico de IA.
    /// </summary>
    public HotelDto[] HotelsIAResult { get; set; } = [];
}
