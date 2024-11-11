// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Redis service settings.
/// </summary>
public sealed class RedisConfig
{
    public const string ConfigSectionName = "Redis";

    [Required]
    public string ConnectionConfiguration { get; set; } = string.Empty;
}
