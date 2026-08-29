using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.ReservationEndpoints;

/// <summary>
/// Controlador para gerenciamento de reservas hoteleiras, criação, cancelamento e consultas por identificador ou quarto.
/// </summary>
[Authorize("Bearer")]
[ApiController]
[Route("api/[controller]/v1")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ReservationsController"/> com o serviço de reservas.
    /// </summary>
    /// <param name="reservationService">Serviço de reservas hoteleiras.</param>
    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>
    /// Define o ID do usuário autenticado no serviço de reservas.
    /// </summary>
    private void SetUserIdCurrent()
    {
        _reservationService.SetUserId(GetUserIdCurrent());
    }

    /// <summary>
    /// Extrai o ID do usuário autenticado a partir das claims do token JWT.
    /// </summary>
    private long GetUserIdCurrent()
    {
        return SecurityHelperApi.GetUserIdApi(User);
    }

    /// <summary>
    /// Obtém uma reserva específica pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador único da reserva.</param>
    /// <returns>DTO contendo os detalhes da reserva.</returns>
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
    /// Cria uma nova reserva após validação de disponibilidade e regras de antecedência.
    /// </summary>
    /// <param name="reservationDto">Dados da reserva a ser criada.</param>
    /// <returns>Resposta contendo os dados da reserva confirmada.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] ReservationDto reservationDto)
    {
        SetUserIdCurrent();
        var response = await _reservationService.CreateAsync(reservationDto);
        return Ok(response);
    }

    /// <summary>
    /// Cancela uma reserva existente aplicando regras de cancelamento.
    /// </summary>
    /// <param name="id">Identificador da reserva a ser cancelada.</param>
    /// <returns>Resposta com status da operação de cancelamento.</returns>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(long id)
    {
        SetUserIdCurrent();
        var response = await _reservationService.CancelReservationAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Obtém todas as reservas associadas a um quarto específico.
    /// </summary>
    /// <param name="roomId">Identificador do quarto.</param>
    /// <returns>Array contendo as reservas do quarto.</returns>
    [HttpGet("room/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByRoomId(long roomId)
    {
        SetUserIdCurrent();
        var reservations = await _reservationService.GetReservationsByRoomIdAsync(roomId);
        if (reservations.Data == null || reservations.Data.Length == 0)
        {
            return NotFound(new { Message = "Nenhuma reserva encontrada para o quarto informado." });
        }
        return Ok(reservations);
    }
}
