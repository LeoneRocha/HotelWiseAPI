using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// DTO com informações de versão e ambiente do produto.
/// Utilizado para expor metadados da aplicação (identidade, nome, versão e ambiente)
/// em endpoints de health, diagnóstico ou about.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.AppInformationVersionProductDto", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.AppInformationVersionProductDto em SmartCoreHub.Core.SDK.")]
public class AppInformationVersionProductDto : SmartCoreHub.Core.SDK.Common.AppInformationVersionProductDto
{

}
