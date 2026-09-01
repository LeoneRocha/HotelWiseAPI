
namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por adapters/serviços de vector store tipados.
/// </summary>
public class SearchCriteria : SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria
{
    /// <summary>Alias legado HW para MaxRetrieve.</summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve;
        set => MaxRetrieve = value;
    }
}
