using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using HotelWise.Core.SDK.Abstractions;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Constants;
using HotelWise.Core.SDK.Helpers;
using HotelWise.Core.SDK.Security;
using HotelWise.Core.SDK.Services;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de aplicação para gerenciamento de usuários, verificação de credenciais e emissão de tokens JWT.
/// </summary>
public class UserService : GenericEntityServiceBase<User, UserLoginDto>, IUserService
{
    private readonly ITokenService _tokenService;
    private readonly ITokenConfigurationDto _configurationToken; 
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UserService"/> com repositório de usuários, token service e configurações.
    /// </summary>
    /// <param name="logger">Logger estruturado.</param>
    /// <param name="repository">Repositório de usuários.</param>
    /// <param name="mapper">Mapeador AutoMapper.</param>
    /// <param name="tokenService">Serviço de geração e validação de tokens JWT.</param>
    /// <param name="configurationToken">Parâmetros de configuração do token JWT.</param>
    /// <param name="entityValidator">Validador de usuário.</param>
    public UserService(
        Serilog.ILogger logger,
        IUserRepository repository,
        IMapper mapper,
        ITokenService tokenService,
        ITokenConfigurationDto configurationToken,
        IValidator<User> entityValidator
    ) : base(repository, mapper, logger, entityValidator)
    {
        _tokenService = tokenService;
        _configurationToken = configurationToken;
        _userRepository = repository;
    }

    /// <summary>
    /// Valida as credenciais de login e senha do usuário e gera os tokens JWT de autenticação.
    /// </summary>
    /// <param name="login">Nome de login do usuário.</param>
    /// <param name="password">Senha em texto simples.</param>
    /// <returns>Resposta contendo o DTO do usuário autenticado e seus tokens.</returns>
    public async Task<ServiceResponse<GetUserAuthenticatedDto>> Login(string login, string password)
    {
        var response = new ServiceResponse<GetUserAuthenticatedDto>();

        // Busca o usuário no repositório
        var user = await _userRepository.FindByLogin(login);
        if (user == null)
        {
            response.Success = false;
            response.Message = ValidatorConstants.Validade_UserNotFound;
            return response;
        }

        // Verifica a senha
        if (!SecurityHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong password.";
            return response;
        }

        // Gera o token e preenche a resposta
        response.Data = await ExecuteLoginJwt(user);
        response.Success = true;
        response.Message = "User logged in successfully.";
        return response;
    }

    /// <summary>
    /// Executa a geração do token JWT e projeta para o DTO de retorno do usuário autenticado.
    /// </summary>
    private async Task<GetUserAuthenticatedDto> ExecuteLoginJwt(User user)
    {
        TokenVO token = await ValidateCredentials(user);

        var response = _mapper.Map<GetUserAuthenticatedDto>(user);
        response.TokenAuth = token;

        return response;
    }

    /// <summary>
    /// Gera claims, access token JWT e refresh token, atualizando a expiração do usuário.
    /// </summary>
    private async Task<TokenVO> ValidateCredentials(User user)
    {
        if (user == null) return new TokenVO();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DataHelper.GetDateTimeNow().AddDays(_configurationToken.DaysToExpiry);

        await _repository.UpdateAsync(user);

        DateTime createDate = DataHelper.GetDateTimeNow();
        DateTime expirationDate = createDate.AddMinutes(_configurationToken.Minutes);

        return new TokenVO(
            true,
            createDate.ToString(AppConfigConstants.DATE_FORMAT2),
            expirationDate.ToString(AppConfigConstants.DATE_FORMAT2),
            accessToken,
            refreshToken
        );
    }
}
