namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de cultura (idioma/região).
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de localização.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.CultureDisplayDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class CultureDisplayDto : SmartCoreHub.Core.SDK.Common.CultureDisplayDto
{

}
