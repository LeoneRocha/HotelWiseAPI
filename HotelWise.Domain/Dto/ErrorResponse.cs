using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HotelWise.Domain.Dto
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// Shim em cópia (sem herança) para zero regressão com consumidores do host.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.Common.ErrorResponse.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_COMMON")]
    public class ErrorResponse
    {
        public string Name { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;

        [Ignore]
        [XmlIgnore]
        [JsonIgnore]
        [SwaggerIgnore]
        [IgnoreDataMember]
        public string DefaultMessage { get; set; } = string.Empty;

        [Ignore]
        [XmlIgnore]
        [JsonIgnore]
        [SwaggerIgnore]
        [IgnoreDataMember]
        public string FullMessage { get; set; } = string.Empty;
    }
}
