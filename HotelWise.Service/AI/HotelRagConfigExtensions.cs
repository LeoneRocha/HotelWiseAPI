using SmartCoreHub.Core.SDK.Domain.AI.Configuration;

namespace HotelWise.Service.AI;

/// <summary>
/// Métodos de extensão de conveniência para resolução do nome de coleção vetorial de hotéis.
/// </summary>
public static class HotelRagConfigExtensions
{
    /// <summary>
    /// Constrói o nome canônico da coleção vetorial respeitando o prefixo e sufixo de entidade informados.
    /// </summary>
    public static string BuildCollectionName(this RagConfig? ragConfig, string entitySuffix) =>
        HotelVectorStoreService.ResolveCollectionName(ragConfig, entitySuffix);
}
