﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns.IA
{
    public enum RoleAiPromptsType
    {
        [JsonPropertyName("system")]
        [Description("system")]
        System = 1,
        [JsonPropertyName("user")]
        [Description("user")]
        User = 2,
        [JsonPropertyName("assistant")]
        [Description("assistant")]
        Assistant = 3,
        [JsonPropertyName("agent")]
        [Description("agent")]
        Agent = 4,
        [JsonPropertyName("Context")]
        [Description("Context")]
        Context = 5
    }
}
