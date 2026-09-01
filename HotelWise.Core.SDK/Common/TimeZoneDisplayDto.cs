using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de fuso horário.
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de configuração.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto em SmartCoreHub.Core.SDK.")]
public class TimeZoneDisplayDto : SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto
{

}
