﻿using System.ComponentModel.DataAnnotations;
using HotelWise.Domain.Interfaces.AppConfig;
namespace HotelWise.Domain.Dto.AppConfig.Rag;

/// <summary>
/// Azure AI Search service settings.
/// </summary>
public abstract class AiInferenceConfigBase : IAiInferenceConfigBase
{
    // Propriedade estática protegida na classe base
    [Required]
    public static string ConfigSectionName { get; protected set; } = string.Empty;

    public string Endpoint { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string ModelId { get; set; } = string.Empty;

    public string? OrgId { get; set; } = null;

    public string EndpointEmbeddings { get; set; } = string.Empty;

    public string ModelIdEmbeddings { get; set; } = string.Empty;
}