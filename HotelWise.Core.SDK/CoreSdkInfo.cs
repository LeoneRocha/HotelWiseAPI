namespace HotelWise.Core.SDK;

/// <summary>
/// Marcador de assembly do HotelWise.Core.SDK (Fase 0 — scaffold).
/// Expõe metadados estáticos do pacote NuGet para identificação em runtime,
/// documentação e pipelines de build. Tipos de domínio serão adicionados
/// nas ondas de migração subsequentes.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.CoreSdkInfo. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class CoreSdkInfo
{
    /// <summary>Identificador do pacote NuGet.</summary>
    public const string PackageId = "HotelWise.Core.SDK";

    /// <summary>Versão do pacote.</summary>
    public const string Version = "1.0.0";
}
