namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por adapters/serviços de vector store tipados.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SearchCriteria : SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria
{
    /// <summary>Alias legado HW para MaxRetrieve.</summary>
    public int MaxHotelRetrieve
    {
        get => MaxRetrieve;
        set => MaxRetrieve = value;
    }
}
