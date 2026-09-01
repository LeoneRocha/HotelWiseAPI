
namespace HotelWise.Core.SDK.AI.Configuration;

/// <summary>
/// Configurações auxiliares de busca vetorial / RAG — herda SCH.
/// </summary>
public class SearchSettings : SmartCoreHub.Core.SDK.Domain.AI.Configuration.SearchSettings
{
    /// <summary>
    /// Limite máximo padrão de registros recuperados na busca vetorial quando não especificado na requisição (configurável via appsettings.json).
    /// </summary>
    public int MaxRetrieve { get; set; } = 50;
}
