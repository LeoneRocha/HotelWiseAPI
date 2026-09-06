using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por adapters/serviços de vector store tipados.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria em SmartCoreHub.Core.SDK.")]
public class SearchCriteria : SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria
{
    /// <summary>
    /// Fallback quando a requisição não informa limite e o appsettings também não.
    /// O valor operacional vem de <c>ApplicationIAConfig:Rag:SearchSettings:MaxRetrieve</c>.
    /// </summary>
    public const int DefaultMaxRetrieve = 25;

    /// <summary>
    /// Alias legado HW para MaxRetrieve.
    /// Valor &lt;= 0 significa "usar appsettings" (não força o default aqui).
    /// </summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve > 0 ? MaxRetrieve : 0;
        set => MaxRetrieve = value > 0 ? value : 0;
    }
}
