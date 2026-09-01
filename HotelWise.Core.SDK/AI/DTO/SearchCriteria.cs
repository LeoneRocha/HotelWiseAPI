
namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por adapters/serviços de vector store tipados.
/// </summary>
public class SearchCriteria : SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria
{
    /// <summary>
    /// Limite padrão elevado para não restringir artificialmente a busca quando não especificado.
    /// </summary>
    public const int DefaultMaxRetrieve = 1000;

    /// <summary>
    /// Construtor garantindo valor mínimo padrão para MaxRetrieve (top > 0).
    /// </summary>
    public SearchCriteria()
    {
        if (MaxRetrieve <= 0)
        {
            MaxRetrieve = DefaultMaxRetrieve;
        }
    }

    /// <summary>Alias legado HW para MaxRetrieve.</summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve > 0 ? MaxRetrieve : DefaultMaxRetrieve;
        set => MaxRetrieve = value > 0 ? value : DefaultMaxRetrieve;
    }
}
