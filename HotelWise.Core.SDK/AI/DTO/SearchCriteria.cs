namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Critérios genéricos de busca vetorial / semântica no pipeline RAG.
/// Usados por <see cref="Abstractions.IVectorStoreAdapter{TVector}.VectorizedSearchAsync"/>
/// e serviços de vector store tipados.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.SearchCriteria. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SearchCriteria
{
    /// <summary>
    /// Quantidade máxima de registros a recuperar na busca.
    /// </summary>
    public int MaxHotelRetrieve { get; set; }

    /// <summary>
    /// Texto da consulta semântica.
    /// </summary>
    public string SearchTextCriteria { get; set; } = string.Empty;

    /// <summary>
    /// Tags usadas para filtrar resultados no vector store.
    /// </summary>
    public string[] TagsCriteria { get; set; } = Array.Empty<string>();
}
