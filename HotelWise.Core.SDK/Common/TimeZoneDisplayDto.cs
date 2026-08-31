namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO de exibição de fuso horário.
/// Representa um item de seleção (identificador e nome amigável) para UI ou APIs de configuração.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class TimeZoneDisplayDto : SmartCoreHub.Core.SDK.Common.TimeZoneDisplayDto
{

}
