using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato de resposta padronizada — casca sobre SCH.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse em SmartCoreHub.Core.SDK.")]
public interface IServiceResponse<T> : SmartCoreHub.Core.SDK.Domain.Abstractions.IServiceResponse<T>
{
}
