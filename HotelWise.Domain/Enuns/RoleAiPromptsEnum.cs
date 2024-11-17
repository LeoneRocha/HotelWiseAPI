using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HotelWise.Domain.Enuns
{
    public enum RoleAiPromptsEnum
    {  
        [JsonPropertyName("system")]
        [Description("system")]
        System = 1, 
        [JsonPropertyName("user")]
        [Description("user")]
        User = 2, 
        [JsonPropertyName("assistant")]
        [Description("assistant")]
        Assistant = 3
    }
}
