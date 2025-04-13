using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Base;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.RoomAvailabilityEndpoints
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class RoomAvailabilityController : ControllerBase
    {
        private readonly IRoomAvailabilityService _roomAvailabilityService;

        public RoomAvailabilityController(IRoomAvailabilityService roomAvailabilityService)
        {
            _roomAvailabilityService = roomAvailabilityService;
        } 
        /// <summary>
        /// Obtém uma disponibilidade pelo ID.
        /// </summary>
        /// <param name="id">ID da disponibilidade</param>
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
  
        [HttpPost("availabilities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAvailabilitiesBySearchCriteriaAsync(RoomAvailabilitySearchDto searchDto)
        {
            var response = await _roomAvailabilityService.GetAvailabilitiesBySearchCriteriaAsync(searchDto);

            if (response == null || response.Data == null || response.Data.Length == 0)
            {
                // Certifique-se de inicializar o objeto 'response' caso ele seja nulo
                response ??= new ServiceResponse<RoomAvailabilityDto[]>(); // Substitua 'YourResponseType' pelo tipo adequado
                response.Message = "Nenhuma disponibilidade encontrada para o quarto informado.";
            } 
            return Ok(response);
        }

         
        /// <summary>
        /// Cria múltiplas disponibilidades em lote.
        /// </summary>
        /// <param name="availabilitiesDto">Array de disponibilidades</param>
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
        /// Deleta uma disponibilidade pelo ID.
        /// </summary>
        /// <param name="id">ID da disponibilidade</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            await _roomAvailabilityService.DeleteAsync(id);
            return Ok(new { Message = "Disponibilidade excluída com sucesso." });
        }
    }
}
