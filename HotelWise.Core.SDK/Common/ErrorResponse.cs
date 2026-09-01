using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#if NET8_0_OR_GREATER
using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
#endif

namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Detalhe de erro padronizado em respostas de serviço.
/// </summary>
public class ErrorResponse : SmartCoreHub.Core.SDK.Common.ErrorResponse
{
}
