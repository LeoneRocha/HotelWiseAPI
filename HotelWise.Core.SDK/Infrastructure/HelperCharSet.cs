using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Constantes de charset — DefaultCharSet permanece latin1 para schema HW legado.
/// Em net8+/net10 aponta ao canônico <c>HelperCharSet.Latin1</c>.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.HelperCharSet", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca HW: DefaultCharSet=Latin1 canônico.")]
public static class HelperCharSet
{
#if NET8_0_OR_GREATER
    /// <summary>Conjunto de caracteres padrão HW (latin1) via canônico SCH.</summary>
    public const string DefaultCharSet = SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.HelperCharSet.Latin1;
#else
    /// <summary>Conjunto de caracteres padrão HW (latin1).</summary>
    public const string DefaultCharSet = "latin1";
#endif
}
