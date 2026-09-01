using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#if NET8_0_OR_GREATER
using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
#endif

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Detalhe de erro padronizado em respostas de serviço.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Common.ErrorResponse", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Common.ErrorResponse em SmartCoreHub.Core.SDK.")]
public class ErrorResponse : SmartCoreHub.Core.SDK.Common.ErrorResponse
{
}
