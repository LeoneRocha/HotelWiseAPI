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
/// Expõe nome, mensagem e código para o cliente; campos internos de mensagem padrão
/// e mensagem completa são ignorados em serialização JSON/XML e no Swagger (quando NET 8+).
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Nome ou chave do erro (campo, regra ou origem).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Mensagem de erro destinada ao consumidor da API.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Código de erro estável para tratamento programático pelo cliente.
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Mensagem padrão interna (não serializada); usada como fallback ou recurso de localização.
    /// </summary>
#if NET8_0_OR_GREATER
    [Ignore]
    [SwaggerIgnore]
#endif
    [XmlIgnore]
    [JsonIgnore]
    [IgnoreDataMember]
    public string DefaultMessage { get; set; } = string.Empty;

    /// <summary>
    /// Mensagem completa interna com detalhes técnicos (não serializada).
    /// </summary>
#if NET8_0_OR_GREATER
    [Ignore]
    [SwaggerIgnore]
#endif
    [XmlIgnore]
    [JsonIgnore]
    [IgnoreDataMember]
    public string FullMessage { get; set; } = string.Empty;
}
