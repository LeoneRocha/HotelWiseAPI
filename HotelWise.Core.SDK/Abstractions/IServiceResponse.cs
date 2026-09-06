using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada — casca sobre Domain.DTOs.Common (não Obsolete Abstractions).
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.DTOs.Common.IServiceResponse`1", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para Domain.DTOs.Common.IServiceResponse.")]
public interface IServiceResponse<T> : SmartCoreHub.Core.SDK.Domain.DTOs.Common.IServiceResponse<T>
{
}
