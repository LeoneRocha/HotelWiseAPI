using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Interfaces.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers;

/// <summary>
/// Controlador responsável pelos fluxos de autenticação, validação de credenciais de login e emissão de tokens JWT.
/// </summary>
[ApiController]
[Route("api/[controller]/v1")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AuthController"/> com o serviço de usuários.
    /// </summary>
    /// <param name="userService">Serviço de gerenciamento e login de usuários.</param>
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Autentica o usuário com credenciais de login e senha, retornando os tokens JWT gerados.
    /// </summary>
    /// <param name="request">Credenciais de login e senha do usuário.</param>
    /// <returns>Resposta com o DTO do usuário autenticado e seus tokens de acesso.</returns>
    [HttpPost("Authenticate")]
    public async Task<ActionResult<ServiceResponse<GetUserAuthenticatedDto>>> Authenticate(UserLoginDto request)
    {
        var response = await _userService.Login(request.Login, request.Password);
        if (!response.Success)
        {
            return Unauthorized(response);
        }
        return Ok(response);
    }
}
