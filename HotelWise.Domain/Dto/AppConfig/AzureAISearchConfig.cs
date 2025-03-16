using System.ComponentModel.DataAnnotations;
namespace HotelWise.Domain.Dto.AppConfig;

/// <summary>
/// Azure AI Search service settings.
/// </summary>
public sealed class AzureAISearchConfig : AiInferenceConfigBase
{
    // Atribuição direta ao membro estático
    public new static string ConfigSectionName => "AzureAISearch";

}