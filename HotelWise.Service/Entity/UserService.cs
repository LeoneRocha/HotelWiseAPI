using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using SmartCoreHub.Core.SDK.Domain.DTOs.Common;
using SmartCoreHub.Core.SDK.Domain.DTOs.Entities;
using SmartCoreHub.Core.SDK.Service.Security;

namespace HotelWise.Service.Entity;

/// <summary>
/// Serviço de aplicação para gerenciamento de usuários, verificação de credenciais e emissão de tokens JWT.
/// </summary>
public class UserService : DtoEntityServiceBase<User, UserLoginDto>, IUserService
{
    private readonly IJwtAccessTokenService _tokenService;
    private readonly ITokenConfigurationDto _configurationToken;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="UserService"/> com repositório de usuários, token service e configurações.
    /// </summary>
    public UserService(
        Serilog.ILogger logger,
        IUserRepository repository,
        IMapper mapper,
        IJwtAccessTokenService tokenService,
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
    public async Task<ServiceResponse<GetUserAuthenticatedDto>> Login(string login, string password)
    {
        var user = await _userRepository.FindByLogin(login);
        if (user == null)
        {
            return ServiceResponse<GetUserAuthenticatedDto>.Error(
                [new ErrorResponse { Message = SmartCoreHub.Core.SDK.Service.Validation.ValidatorConstants.Validade_UserNotFound }],
                SmartCoreHub.Core.SDK.Service.Validation.ValidatorConstants.Validade_UserNotFound);
        }

        if (!PasswordHashHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return ServiceResponse<GetUserAuthenticatedDto>.Error(
                [new ErrorResponse { Message = "Wrong password." }],
                "Wrong password.");
        }

        var data = await ExecuteLoginJwt(user);
        return ServiceResponse<GetUserAuthenticatedDto>.Ok(data, "User logged in successfully.");
    }

    private async Task<GetUserAuthenticatedDto> ExecuteLoginJwt(User user)
    {
        TokenVO token = await ValidateCredentials(user);

        var response = _mapper.Map<GetUserAuthenticatedDto>(user);
        response.TokenAuth = token;

        return response;
    }

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
