namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com dados de segurança usados na geração e contextualização de tokens.
/// Transporta identidade, papel e chave de configuração associada ao usuário autenticado.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.SecurityDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class SecurityDto : SmartCoreHub.Core.SDK.Common.SecurityDto
{

}
