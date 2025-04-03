using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.ReservationEndpoints
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        private void SetUserIdCurrent()
        {
            _reservationService.SetUserId(GetUserIdCurrent());
        }

        private long GetUserIdCurrent()
        {
            return SecurityHelperApi.GetUserIdApi(User);
        } 

        /// <summary>
        /// Obtém uma reserva pelo ID.
        /// </summary>
        /// <param name="id">ID da reserva</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            SetUserIdCurrent();
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound(new { Message = "Reserva não encontrada." });
            }
            return Ok(reservation);
        }

        /// <summary>
        /// Cria uma nova reserva.
        /// </summary>
        /// <param name="reservationDto">Dados da reserva</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ReservationDto reservationDto)
        {
            SetUserIdCurrent();
            var response = await _reservationService.CreateAsync(reservationDto);
            return Ok(response);
        }
          
        /// <summary>
        /// Cancela uma reserva pelo ID.
        /// </summary>
        /// <param name="id">ID da reserva</param>
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel(long id)
        {
            SetUserIdCurrent();
            var response = await _reservationService.CancelReservationAsync(id);
            return Ok(response);
        }
         
        /// <summary>
        /// Obtém reservas associadas a um quarto específico.
        /// </summary>
        /// <param name="roomId">ID do quarto</param>
        [HttpGet("room/{roomId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByRoomId(long roomId)
        {
            SetUserIdCurrent();
            var reservations = await _reservationService.GetReservationsByRoomIdAsync(roomId);
            if (reservations.Data == null || !reservations.Data.Any())
            {
                return NotFound(new { Message = "Nenhuma reserva encontrada para o quarto informado." });
            }
            return Ok(reservations);
        }
    }
}
