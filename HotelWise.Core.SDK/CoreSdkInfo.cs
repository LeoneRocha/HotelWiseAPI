using SchCommon = SmartCoreHub.Core.SDK.Common;

namespace HotelWise.Core.SDK;

/// <summary>
/// Marcador de assembly do HotelWise.Core.SDK — delega metadados canônicos ao SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.CoreSdkInfo. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class CoreSdkInfo
{
    /// <summary>Identificador do pacote NuGet canônico (SCH).</summary>
    public const string PackageId = SchCommon.CoreSdkInfo.PackageId;

    /// <summary>Versão de referência do pacote canônico (SCH).</summary>
    public const string Version = SchCommon.CoreSdkInfo.Version;
}
