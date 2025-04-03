using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.RoomEndpoints
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        private void setUserIdCurrent()
        {
            _roomService.SetUserId(GetUserIdCurrent());
        }

        private long GetUserIdCurrent()
        {
            return SecurityHelperApi.GetUserIdApi(User);
        }
          
        /// <summary>
        /// Obtém um quarto pelo ID.
        /// </summary>
        /// <param name="id">ID do quarto</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            setUserIdCurrent();
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        /// <summary>
        /// Cria um novo quarto.
        /// </summary>
        /// <param name="room">Dados do quarto</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] RoomDto room)
        {
            setUserIdCurrent();
            var response = await _roomService.CreateAsync(room);
            return Ok(response);
        }

        /// <summary>
        /// Atualiza um quarto existente.
        /// </summary>
        /// <param name="id">ID do quarto</param>
        /// <param name="room">Dados atualizados do quarto</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] RoomDto room)
        {
            if (id != room.Id)
            {
                return BadRequest();
            }
            setUserIdCurrent();
            var response = await _roomService.UpdateAsync(room);
            return Ok(response);
        }

        /// <summary>
        /// Deleta um quarto pelo ID.
        /// </summary>
        /// <param name="id">ID do quarto</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            setUserIdCurrent();
            await _roomService.DeleteAsync(id);
            return Ok(new { Message = "Quarto excluído com sucesso." });
        }
    }
}
