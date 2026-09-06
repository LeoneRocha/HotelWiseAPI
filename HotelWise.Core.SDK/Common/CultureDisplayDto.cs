
using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de cultura (idioma/região).
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de localização.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto em SmartCoreHub.Core.SDK.")]
public class CultureDisplayDto : SmartCoreHub.Core.SDK.Domain.DTOs.Entities.CultureDisplayDto
{

}
