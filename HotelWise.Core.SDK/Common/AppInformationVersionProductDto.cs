namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com informações de versão e ambiente do produto.
/// Utilizado para expor metadados da aplicação (identidade, nome, versão e ambiente)
/// em endpoints de health, diagnóstico ou about.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.AppInformationVersionProductDto. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AppInformationVersionProductDto : SmartCoreHub.Core.SDK.Common.AppInformationVersionProductDto
{

}
