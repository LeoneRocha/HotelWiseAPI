namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato base de configuração de inferência IA.
/// </summary>
public interface IAiInferenceConfigBase
{
    string ApiKey { get; set; }
    string Endpoint { get; set; }
    string ModelId { get; set; }
    string? OrgId { get; set; }
    string EndpointEmbeddings { get; set; }
    string ModelIdEmbeddings { get; set; }
}
