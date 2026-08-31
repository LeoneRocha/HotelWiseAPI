namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configurações auxiliares de busca vetorial / RAG — herda SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SearchSettings : SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings
{
}
