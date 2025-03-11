using AutoMapper;
using HotelWise.Domain.Constants;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using HotelWise.Service.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HotelWise.Service.Entity
{
    public class UserService : GenericServiceBase<User, UserLoginDto>, IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenConfigurationDto _configurationToken;

        private readonly IUserRepository _userRepository;

        public UserService(
              Serilog.ILogger logger,
            IUserRepository repository,
            IMapper mapper,
            ITokenService tokenService,
            ITokenConfigurationDto configurationToken
        ) : base(repository, mapper, logger)
        {
            _tokenService = tokenService;
            _configurationToken = configurationToken;
            _userRepository = repository;
        }

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

        private async Task<GetUserAuthenticatedDto> ExecuteLoginJwt(User user)
        {
            // Valida as credenciais e gera o token
            TokenVO token = await ValidateCredentials(user);

            // Mapeia o usuário para o DTO e adiciona o token
            var response = _mapper.Map<GetUserAuthenticatedDto>(user);
            response.TokenAuth = token;

            return response;
        }

        private async Task<TokenVO> ValidateCredentials(User user)
        {
            if (user == null) return new TokenVO();

            // Gera as claims para o token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
            };

            // Gera o access token e o refresh token
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Atualiza o usuário com o token de atualização e seu prazo de expiração
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DataHelper.GetDateTimeNow().AddDays(_configurationToken.DaysToExpiry);

            await _repository.UpdateAsync(user);

            // Cria o objeto TokenVO com informações do token
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
}
