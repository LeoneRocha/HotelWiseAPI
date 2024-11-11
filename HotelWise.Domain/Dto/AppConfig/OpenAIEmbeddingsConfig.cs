// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// OpenAI Embeddings service settings.
/// </summary>
public sealed class OpenAIEmbeddingsConfig
{
    public const string ConfigSectionName = "OpenAIEmbeddings";

    [Required]
    public string ModelId { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;

    [Required]
    public string? OrgId { get; set; } = null;
}
