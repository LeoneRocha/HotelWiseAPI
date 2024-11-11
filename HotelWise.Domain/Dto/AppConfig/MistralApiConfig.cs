// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Qdrant service settings.
/// </summary>
public sealed class MistralApiConfig
{
    public const string ConfigSectionName = "MistralApí";
     
    [Required]
    public string ModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string? OrgId { get; set; } = null;
}
