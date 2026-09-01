using SchCommon = SmartCoreHub.Core.SDK.Common;

namespace HotelWise.Core.SDK;

/// <summary>
/// Marcador de assembly do HotelWise.Core.SDK — delega metadados canônicos ao SCH.
/// </summary>
public static class CoreSdkInfo
{
    /// <summary>Identificador do pacote NuGet canônico (SCH).</summary>
    public const string PackageId = SchCommon.CoreSdkInfo.PackageId;

    /// <summary>Versão de referência do pacote canônico (SCH).</summary>
    public const string Version = SchCommon.CoreSdkInfo.Version;
}
