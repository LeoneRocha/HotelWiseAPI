using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Resposta padronizada de operações de serviço — herda Domain.DTOs.Common (não Common Obsolete).
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse`1", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper sobre Domain.DTOs.Common.ServiceResponse.")]
public class ServiceResponse<T> : SmartCoreHub.Core.SDK.Domain.DTOs.Common.ServiceResponse<T>, HotelWise.Core.SDK.Abstractions.IServiceResponse<T>
{
}
