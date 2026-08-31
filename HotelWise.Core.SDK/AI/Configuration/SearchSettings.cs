namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configurações auxiliares de busca vetorial / RAG.
/// Controla atrasos e comportamentos relacionados à consulta no vector store.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SearchSettings
{
    /// <summary>
    /// Atraso em milissegundos antes de executar a busca (ex.: após indexação).
    /// </summary>
    public int DelayBeforeSearchMilliseconds { get; set; }
}
