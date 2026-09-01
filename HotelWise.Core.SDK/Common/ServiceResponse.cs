using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Resposta padronizada de operações de serviço.
/// </summary>
/// <typeparam name="T">Tipo do payload de dados retornado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.ServiceResponse", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.ServiceResponse em SmartCoreHub.Core.SDK.")]
public class ServiceResponse<T> : SmartCoreHub.Core.SDK.Common.ServiceResponse<T>, HotelWise.Core.SDK.Abstractions.IServiceResponse<T>
{
}
