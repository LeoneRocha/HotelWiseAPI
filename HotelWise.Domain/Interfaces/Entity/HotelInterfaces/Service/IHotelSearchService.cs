using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Generic;
using SmartCoreHub.Core.SDK.Domain.DTOs.Common;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço especializado em busca semântica e contextual de hotéis utilizando inteligência artificial e RAG.
/// </summary>
public interface IHotelSearchService : IGenericDtoService<HotelDto>
{
    /// <summary>
    /// Executa uma busca semântica inteligente sobre a base de hotéis a partir dos critérios fornecidos.
    /// </summary>
    /// <param name="searchCriteria">Critérios de busca contendo consulta textual, filtros e limite de resultados.</param>
    /// <returns>Resposta contendo o resultado semântico com análise de IA e hotéis encontrados.</returns>
    Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria);
}
