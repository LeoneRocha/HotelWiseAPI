using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.RoomEndpoints;

/// <summary>
/// Controlador para gerenciamento completo do cadastro de quartos dos hotéis.
/// </summary>
[Authorize("Bearer")]
[ApiController]
[Route("api/[controller]/v1")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="RoomsController"/> com o serviço de quartos.
    /// </summary>
    /// <param name="roomService">Serviço de quartos.</param>
    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Define o ID do usuário autenticado no serviço de quartos.
    /// </summary>
    private void setUserIdCurrent()
    {
        _roomService.SetUserId(GetUserIdCurrent());
    }

    /// <summary>
    /// Extrai o ID do usuário autenticado a partir das claims do token JWT.
    /// </summary>
    private long GetUserIdCurrent()
    {
        return SecurityHelperApi.GetUserIdApi(User);
    }

    /// <summary>
    /// Obtém um quarto específico pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador único do quarto.</param>
    /// <returns>DTO com os dados do quarto.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        setUserIdCurrent();
        var room = await _roomService.GetByIdAsync(id);
        if (room == null)
        {
            return NotFound(new { Message = "Quarto não encontrado." });
        }
        return Ok(room);
    }

    /// <summary>
    /// Pesquisa e lista todos os quartos cadastrados para um hotel específico.
    /// </summary>
    /// <param name="hotelId">Identificador do hotel.</param>
    /// <returns>Array contendo os quartos vinculados ao hotel.</returns>
    [HttpGet("hotel/{hotelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomsByHotel(long hotelId)
    {
        setUserIdCurrent();
        var response = await _roomService.GetRoomsByHotelIdAsync(hotelId);
        if (response == null)
        {
            return NotFound(new { Message = "Nenhum quarto encontrado para o hotel informado." });
        }
        else if (response.Data == null || response.Data.Length == 0)
        {
            response.Message = "Nenhum quarto encontrado para o hotel informado.";
            return Ok(response);
        }

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo quarto no hotel.
    /// </summary>
    /// <param name="room">Dados do quarto a cadastrar.</param>
    /// <returns>Resposta contendo o DTO do quarto cadastrado.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] RoomDto room)
    {
        setUserIdCurrent();
        var response = await _roomService.CreateAsync(room);
        return Ok(response);
    }

    /// <summary>
    /// Atualiza os dados cadastrais de um quarto existente.
    /// </summary>
    /// <param name="id">Identificador do quarto.</param>
    /// <param name="room">Dados atualizados do quarto.</param>
    /// <returns>Resposta contendo o DTO do quarto atualizado.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] RoomDto room)
    {
        if (id != room.Id)
        {
            return BadRequest(new { Message = "O ID do quarto não corresponde ao fornecido." });
        }
        setUserIdCurrent();
        var response = await _roomService.UpdateAsync(room);
        return Ok(response);
    }

    /// <summary>
    /// Exclui um quarto do sistema pelo seu identificador primário.
    /// </summary>
    /// <param name="id">Identificador do quarto a ser excluído.</param>
    /// <returns>Confirmação da exclusão.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(long id)
    {
        setUserIdCurrent();
        await _roomService.DeleteAsync(id);
        return Ok(new { Message = "Quarto excluído com sucesso." });
    }
}

