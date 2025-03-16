using System.ComponentModel.DataAnnotations;
using HotelWise.Domain.Interfaces.AppConfig;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure AI Search service settings.
/// </summary>
public abstract class AiInferenceConfigBase : IAiInferenceConfigBase
{
    // Propriedade estática protegida na classe base
    public static string ConfigSectionName { get; protected set; } = string.Empty;

    [Required]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
     
    [Required]
    public string ModelId { get; set; } = string.Empty;

    [Required]
    public string? OrgId { get; set; } = null;
}