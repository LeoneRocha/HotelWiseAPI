using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por adapters/serviços de vector store tipados.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria em SmartCoreHub.Core.SDK.")]
public class SearchCriteria : SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria
{
    /// <summary>Alias legado HW para MaxRetrieve.</summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve;
        set => MaxRetrieve = value;
    }
}
