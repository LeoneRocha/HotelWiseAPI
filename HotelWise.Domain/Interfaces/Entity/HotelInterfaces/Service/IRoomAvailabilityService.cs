using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;

/// <summary>
/// Contrato de serviço para gerenciamento e consulta de disponibilidade e precificação de quartos.
/// </summary>
public interface IRoomAvailabilityService : IGenericService<RoomAvailabilityDto>
{ 
    /// <summary>
    /// Recupera todas as disponibilidades cadastradas para um determinado quarto.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Resposta contendo o array de disponibilidades do quarto.</returns>
    Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesByRoomIdAsync(long roomId);

    /// <summary>
    /// Cadastra um lote de disponibilidades de quartos em uma única operação.
    /// </summary>
    /// <param name="availabilitiesDto">Array de DTOs de disponibilidade a cadastrar.</param>
    /// <returns>Resposta contendo o resultado da inserção em lote.</returns>
    Task<ServiceResponse<string>> CreateBatchAsync(RoomAvailabilityDto[] availabilitiesDto);

    /// <summary>
    /// Consulta as disponibilidades de quartos atendendo aos critérios de período e hotel informados.
    /// </summary>
    /// <param name="searchDto">Critérios de busca por hotel e intervalo de datas.</param>
    /// <returns>Resposta contendo o array de disponibilidades correspondentes.</returns>
    Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesBySearchCriteriaAsync(RoomAvailabilitySearchDto searchDto);
}
