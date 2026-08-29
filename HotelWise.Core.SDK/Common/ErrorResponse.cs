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
public class ErrorResponse
{
    public string Name { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string ErrorCode { get; set; } = string.Empty;

#if NET8_0_OR_GREATER
    [Ignore]
    [SwaggerIgnore]
#endif
    [XmlIgnore]
    [JsonIgnore]
    [IgnoreDataMember]
    public string DefaultMessage { get; set; } = string.Empty;

#if NET8_0_OR_GREATER
    [Ignore]
    [SwaggerIgnore]
#endif
    [XmlIgnore]
    [JsonIgnore]
    [IgnoreDataMember]
    public string FullMessage { get; set; } = string.Empty;
}
