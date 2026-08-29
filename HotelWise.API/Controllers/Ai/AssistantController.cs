using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Security;
using HotelWise.Domain.Interfaces.Entity.IA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers.Ai;

/// <summary>
/// Controlador para interação com o assistente inteligente StayMate via chat completion contextual.
/// </summary>
[Authorize("Bearer")]
[ApiController]
[Route("api/[controller]/v1")]
public class AssistantController : ControllerBase
{
    private readonly IAssistantService _assistantService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AssistantController"/> com o serviço de assistente conversacional.
    /// </summary>
    /// <param name="assistantService">Serviço do assistente inteligente.</param>
    public AssistantController(IAssistantService assistantService)
    {
        _assistantService = assistantService;
    }

    /// <summary>
    /// Define o ID do usuário autenticado no serviço de assistente.
    /// </summary>
    private void setUserIdCurrent()
    {
        _assistantService.SetUserId(GetUserIdCurrent());
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
    /// Envia uma mensagem ou pergunta para o assistente StayMate e retorna a resposta gerada com histórico atualizado.
    /// </summary>
    /// <param name="request">Requisição contendo a mensagem do usuário e o token de sessão opcional.</param>
    /// <returns>Array contendo as respostas geradas pelo assistente de IA.</returns>
    [HttpPost("ask")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AskAssistant([FromBody] AskAssistantRequest request)
    {
        setUserIdCurrent();
        var result = await _assistantService.AskAssistant(request);
        if (result != null && result.Length > 0)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest();
        }
    }
}