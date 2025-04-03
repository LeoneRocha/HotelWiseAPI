using HotelWise.Domain.Dto.Enitty.HotelDtos;
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
                return NotFound();
            }
            return Ok(availability);
        }

        /// <summary>
        /// Cria uma nova disponibilidade.
        /// </summary>
        /// <param name="availabilityDto">Dados da disponibilidade</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RoomAvailabilityDto availabilityDto)
        {
            var response = await _roomAvailabilityService.CreateAsync(availabilityDto);
            return Ok(response);
        }

        /// <summary>
        /// Atualiza uma disponibilidade existente.
        /// </summary>
        /// <param name="id">ID da disponibilidade</param>
        /// <param name="availabilityDto">Dados atualizados</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] RoomAvailabilityDto availabilityDto)
        {
            if (id != availabilityDto.Id)
            {
                return BadRequest();
            }
            var response = await _roomAvailabilityService.UpdateAsync(availabilityDto);
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
