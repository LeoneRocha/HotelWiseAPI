using SchCommon = SmartCoreHub.Core.SDK.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK;

/// <summary>
/// Marcador de assembly do HotelWise.Core.SDK — delega metadados canônicos ao SCH.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.CoreSdkInfo", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.CoreSdkInfo em SmartCoreHub.Core.SDK.")]
public static class CoreSdkInfo
{
    /// <summary>Identificador do pacote NuGet canônico (SCH).</summary>
    public const string PackageId = SchCommon.CoreSdkInfo.PackageId;

    /// <summary>Versão de referência do pacote canônico (SCH).</summary>
    public const string Version = SchCommon.CoreSdkInfo.Version;
}
