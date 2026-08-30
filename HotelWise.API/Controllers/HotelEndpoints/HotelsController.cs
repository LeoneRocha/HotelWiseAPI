using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.HotelEndpoints;

/// <summary>
/// Controlador para gerenciamento completo do ciclo de vida de hotéis, indexação vetorial, geração sintética e busca semântica RAG.
/// </summary>
[Authorize("Bearer")]
[ApiController]
[Route("api/[controller]/v1")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly IHotelSearchService _hotelSearchService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="HotelsController"/> com os serviços de hotel e busca semântica.
    /// </summary>
    /// <param name="hotelService">Serviço de gerenciamento de hotéis.</param>
    /// <param name="hotelSearchService">Serviço de busca semântica.</param>
    public HotelsController(IHotelService hotelService, IHotelSearchService hotelSearchService)
    {
        _hotelService = hotelService;
        _hotelSearchService = hotelSearchService;
    }

    /// <summary>
    /// Define o ID do usuário autenticado no serviço de hotéis.
    /// </summary>
    private void setUserIdCurrent()
    {
        _hotelService.SetUserId(GetUserIdCurrent());
    }

    /// <summary>
    /// Extrai o ID do usuário autenticado a partir das claims do token JWT.
    /// </summary>
    private long GetUserIdCurrent()
    {
        long idUser = SecurityHelperApi.GetUserIdApi(User);
        return idUser;
    }

    /// <summary>
    /// Obtém a listagem de todos os hotéis cadastrados no sistema.
    /// </summary>
    /// <returns>Lista de DTOs de hotéis cadastrados.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        setUserIdCurrent();
        var hotels = await _hotelService.GetAllHotelsAsync();
        return Ok(hotels);
    }

    /// <summary>
    /// Obtém as informações detalhadas de um hotel pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>DTO com as informações do hotel.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        setUserIdCurrent();
        var hotel = await _hotelService.GetHotelByIdAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }
        return Ok(hotel);
    }

    /// <summary>
    /// Obtém todas as tags únicas associadas a todos os hotéis cadastrados.
    /// </summary>
    /// <returns>Array contendo os nomes de todas as tags existentes.</returns>
    [HttpGet("tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTags()
    {
        setUserIdCurrent();
        string[] tags = await _hotelService.GetAllTags();
        return Ok(tags);
    }

    /// <summary>
    /// Sincroniza e insere o hotel informado na base vetorial (Vector Store).
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <returns>Booleano indicando o sucesso da operação.</returns>
    [HttpGet("addvector/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddVectorById(long id)
    {
        setUserIdCurrent();
        var result = await _hotelService.InsertHotelInVectorStore(id);
        return Ok(result);
    }

    /// <summary>
    /// Gera sinteticamente um hotel com descrições, tags e características usando IA.
    /// </summary>
    /// <returns>DTO contendo o hotel gerado.</returns>
    [HttpGet("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Generate()
    {
        setUserIdCurrent();
        var hotel = await _hotelService.GenerateHotelByIA();
        return Ok(hotel);
    }

    /// <summary>
    /// Executa uma pesquisa semântica inteligente utilizando Vector Store e agente de viagens StayMate.
    /// </summary>
    /// <param name="searchCriteria">Critérios e filtros de pesquisa.</param>
    /// <returns>Resultado da pesquisa semântica enriquecida por IA.</returns>
    [HttpPost("semanticsearch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SemanticSearch([FromBody] SearchCriteria searchCriteria)
    {
        setUserIdCurrent();
        var hotels = await _hotelSearchService.SemanticSearch(searchCriteria);
        return Ok(hotels);
    }

    /// <summary>
    /// Cadastra um novo hotel e gera seu embedding na base vetorial.
    /// </summary>
    /// <param name="hotel">Dados do hotel a ser cadastrado.</param>
    /// <returns>Resultado da operação de criação.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] HotelDto hotel)
    {
        setUserIdCurrent();
        var response = await _hotelService.AddHotelAsync(hotel);

        return Ok(response);
    }

    /// <summary>
    /// Atualiza as informações cadastrais de um hotel existente.
    /// </summary>
    /// <param name="id">Identificador do hotel.</param>
    /// <param name="hotel">Dados atualizados do hotel.</param>
    /// <returns>Resultado da operação de atualização.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] HotelDto hotel)
    {
        if (id != hotel.HotelId)
        {
            return BadRequest();
        }
        setUserIdCurrent();
        var response = await _hotelService.UpdateHotelAsync(hotel);
        return Ok(response);
    }

    /// <summary>
    /// Remove um hotel da base de dados e seu vetor correspondente.
    /// </summary>
    /// <param name="id">Identificador do hotel a ser removido.</param>
    /// <returns>Resultado da operação de exclusão.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(long id)
    {
        setUserIdCurrent();
        var response = await _hotelService.DeleteHotelAsync(id);
        return Ok(response);
    }
}

