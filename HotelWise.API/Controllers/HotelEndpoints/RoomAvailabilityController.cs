using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.RoomAvailabilityEndpoints;

/// <summary>
/// Controlador para gerenciamento de disponibilidades e tarifas de quartos, incluindo criação/atualização em lote.
/// </summary>
[Authorize("Bearer")]
[ApiController]
[Route("api/[controller]/v1")]
public class RoomAvailabilityController : ControllerBase
{
    private readonly IRoomAvailabilityService _roomAvailabilityService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomAvailabilityController"/> com o serviço de disponibilidades.
    /// </summary>
    /// <param name="roomAvailabilityService">Serviço de disponibilidades de quartos.</param>
    public RoomAvailabilityController(IRoomAvailabilityService roomAvailabilityService)
    {
        _roomAvailabilityService = roomAvailabilityService;
    }

    /// <summary>
    /// Obtém uma disponibilidade de quarto pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador da disponibilidade.</param>
    /// <returns>DTO da disponibilidade encontrada.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var availability = await _roomAvailabilityService.GetByIdAsync(id);
        if (availability == null)
        {
            return NotFound(new { Message = "Disponibilidade não encontrada." });
        }
        return Ok(availability);
    }

    /// <summary>
    /// Pesquisa disponibilidades e preços por critérios de hotel e período.
    /// </summary>
    /// <param name="searchDto">Critérios de busca por hotel e período.</param>
    /// <returns>Array de disponibilidades encontradas.</returns>
    [HttpPost("availabilities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailabilitiesBySearchCriteriaAsync(RoomAvailabilitySearchDto searchDto)
    {
        var response = await _roomAvailabilityService.GetAvailabilitiesBySearchCriteriaAsync(searchDto);

        if (response == null || response.Data == null || response.Data.Length == 0)
        {
            response ??= new ServiceResponse<RoomAvailabilityDto[]>();
            response.Message = "Nenhuma disponibilidade encontrada para o quarto informado.";
        }
        return Ok(response);
    }

    /// <summary>
    /// Cria ou atualiza múltiplas disponibilidades de quartos em lote.
    /// </summary>
    /// <param name="availabilitiesDto">Array com as disponibilidades a criar ou atualizar.</param>
    /// <returns>Resultado da operação em lote.</returns>
    [HttpPost("batch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateBatch([FromBody] RoomAvailabilityDto[] availabilitiesDto)
    {
        if (availabilitiesDto == null || availabilitiesDto.Length == 0)
        {
            var result = new ServiceResponse<string>() { Data = "Nenhuma disponibilidade fornecida.", Message = "Nenhuma disponibilidade fornecida." };
            return Ok(result);
        }
        var response = await _roomAvailabilityService.CreateBatchAsync(availabilitiesDto);
        return Ok(response);
    }

    /// <summary>
    /// Remove uma disponibilidade de quarto pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador da disponibilidade.</param>
    /// <returns>Confirmação da exclusão.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(long id)
    {
        await _roomAvailabilityService.DeleteAsync(id);
        return Ok(new { Message = "Disponibilidade excluída com sucesso." });
    }
}
